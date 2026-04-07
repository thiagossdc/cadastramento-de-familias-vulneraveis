/**
 * Store Pinia - Estado Global Persistente
 * Persistência automática no LocalStorage com debounce
 */

import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useLocalStorage } from '@vueuse/core'
import { debounce } from '@vueuse/core'

import type { CreateFormRequest, FormSection } from '../types/form'
import { INITIAL_FORM_STATE, FORM_SECTIONS } from '../types/form'
import formApi from '../services/api'

export const useFormStore = defineStore('form', () => {
  // Estado persistente no LocalStorage
  const data = useLocalStorage<CreateFormRequest>('form_data', structuredClone(INITIAL_FORM_STATE), {
    mergeDefaults: true
  })

  const isDirty = ref(false)
  const isLoading = ref(false)
  const currentSection = ref<FormSection>('cadastral')
  const expandedSections = ref<FormSection[]>(['cadastral'])
  const lastSavedAt = ref<string | null>(null)
  const errors = ref<Record<string, string>>({})

  // Getters computados
  const progress = computed(() => {
    const total = Object.keys(FORM_SECTIONS).length
    const completed = Object.values(FORM_SECTIONS).filter(s => {
      switch (s.id) {
        case 'cadastral': return data.value.cadastralData.applicantName.length > 0
        case 'family': return data.value.familyMembers.length > 0
        default: return false
      }
    }).length
    return Math.round((completed / total) * 100)
  })

  // Debounce persistência (500ms)
  const persistDebounced = debounce(() => {
    isDirty.value = true
  }, 500)

  // Actions
  function updateField(path: string, value: unknown) {
    const keys = path.split('.')
    let target: any = data.value

    for (let i = 0; i < keys.length - 1; i++) {
      target = target[keys[i]]
    }

    target[keys[keys.length - 1]] = value
    persistDebounced()
  }

  function addFamilyMember() {
    data.value.familyMembers.push({
      name: '',
      age: 0,
      relationship: '',
      occupation: '',
      schooling: ''
    })
    persistDebounced()
  }

  function removeFamilyMember(index: number) {
    data.value.familyMembers.splice(index, 1)
    persistDebounced()
  }

  function toggleSection(section: FormSection) {
    const index = expandedSections.value.indexOf(section)
    if (index === -1) {
      expandedSections.value.push(section)
    } else {
      expandedSections.value.splice(index, 1)
    }
  }

  async function submitForm() {
    isLoading.value = true
    errors.value = {}

    try {
      const result = await formApi.create(data.value)
      lastSavedAt.value = new Date().toISOString()
      isDirty.value = false
      return result
    } catch (error: any) {
      errors.value.general = error.message
      throw error
    } finally {
      isLoading.value = false
    }
  }

  function resetForm() {
    Object.assign(data.value, structuredClone(INITIAL_FORM_STATE))
    isDirty.value = false
    errors.value = {}
    currentSection.value = 'cadastral'
    expandedSections.value = ['cadastral']
    lastSavedAt.value = null
  }

  return {
    // Estado
    data,
    isDirty,
    isLoading,
    currentSection,
    expandedSections,
    lastSavedAt,
    errors,
    // Getters
    progress,
    // Actions
    updateField,
    addFamilyMember,
    removeFamilyMember,
    toggleSection,
    submitForm,
    resetForm
  }
})