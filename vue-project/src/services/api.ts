/**
 * Camada de Serviço API
 * Cliente HTTP com retry, cache, timeout e tratamento de erros
 */

import type { CreateFormRequest, FormResponse } from '../types/form'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

interface ApiConfig {
  retries: number
  retryDelay: number
  timeout: number
  cacheTTL: number
}

const DEFAULT_CONFIG: ApiConfig = {
  retries: 3,
  retryDelay: 1000,
  timeout: 10000,
  cacheTTL: 300000
}

const cache = new Map<string, { data: unknown; timestamp: number }>()

async function delay(ms: number): Promise<void> {
  return new Promise(resolve => setTimeout(resolve, ms))
}

async function fetchWithRetry<T>(
  url: string,
  options: RequestInit = {},
  config: Partial<ApiConfig> = {}
): Promise<T> {
  const { retries, retryDelay, timeout } = { ...DEFAULT_CONFIG, ...config }
  const controller = new AbortController()
  const timeoutId = setTimeout(() => controller.abort(), timeout)

  try {
    for (let attempt = 0; attempt <= retries; attempt++) {
      try {
        const response = await fetch(url, {
          ...options,
          signal: controller.signal,
          headers: {
            'Content-Type': 'application/json',
            ...options.headers
          }
        })

        if (!response.ok) {
          const error = await response.json().catch(() => ({ message: 'Erro desconhecido' }))
          throw new Error(error.message || `HTTP ${response.status}`)
        }

        return await response.json()
      } catch (error) {
        if (attempt === retries || error instanceof Error && error.name === 'AbortError') {
          throw error
        }
        await delay(retryDelay * Math.pow(2, attempt))
      }
    }
    throw new Error('Número máximo de tentativas excedido')
  } finally {
    clearTimeout(timeoutId)
  }
}

export const formApi = {
  async create(formData: CreateFormRequest): Promise<FormResponse> {
    return fetchWithRetry<FormResponse>(`${API_BASE_URL}/forms`, {
      method: 'POST',
      body: JSON.stringify(formData)
    })
  },

  async getById(id: number): Promise<FormResponse> {
    const cacheKey = `form:${id}`
    const cached = cache.get(cacheKey)

    if (cached && Date.now() - cached.timestamp < DEFAULT_CONFIG.cacheTTL) {
      return cached.data as FormResponse
    }

    const data = await fetchWithRetry<FormResponse>(`${API_BASE_URL}/forms/${id}`)
    cache.set(cacheKey, { data, timestamp: Date.now() })
    return data
  },

  async update(id: number, formData: Partial<CreateFormRequest>): Promise<FormResponse> {
    cache.delete(`form:${id}`)
    return fetchWithRetry<FormResponse>(`${API_BASE_URL}/forms/${id}`, {
      method: 'PUT',
      body: JSON.stringify(formData)
    })
  },

  async delete(id: number): Promise<void> {
    cache.delete(`form:${id}`)
    return fetchWithRetry(`${API_BASE_URL}/forms/${id}`, {
      method: 'DELETE'
    })
  },

  async getAll(params: { page?: number; limit?: number } = {}): Promise<{ items: FormResponse[]; total: number }> {
    const query = new URLSearchParams(params as Record<string, string>)
    return fetchWithRetry(`${API_BASE_URL}/forms?${query}`)
  },

  clearCache(): void {
    cache.clear()
  }
}

export default formApi