/**
 * Tipos 1:1 com DTOs do Backend .NET
 * Sincronia perfeita com CreateFormRequest.cs
 */

export interface CadastralData {
  applicantName: string
  cpf: string
  rg: string
  birthDate: string
  maritalStatus: string
  address: string
  number: string
  neighborhood: string
  city: string
  state: string
  cep: string
  phone: string
  email: string
  familyMembersCount: number
}

export interface FamilyMember {
  name: string
  age: number
  relationship: string
  occupation: string
  schooling: string
}

export interface EducationData {
  schoolingLevel: string
  isAttendingSchool: boolean
  course: string
  hasProfessionalTraining: boolean
  trainingArea: string
}

export interface IncomeExpensesData {
  monthlyFamilyIncome: number
  totalExpenses: number
  rentValue: number
  receivesGovernmentBenefit: boolean
  benefitType: string
  benefitValue: number
}

export interface HousingData {
  housingType: string
  ownProperty: boolean
  roomsCount: number
  bedroomsCount: number
  hasBasicSanitation: boolean
  hasElectricity: boolean
  hasRunningWater: boolean
}

export interface CreateFormRequest {
  cadastralData: CadastralData
  familyMembers: FamilyMember[]
  educationData: EducationData
  familyNeeds: string[]
  essentialItems: string[]
  healthNeeds: string
  hasProfessionalSkills: boolean
  occupation: string
  experienceTime: string
  occupationOther: string
  skills: string
  socialIssues: string[]
  otherIssues: string
  incomeExpensesData: IncomeExpensesData
  housingData: HousingData
}

export interface FormResponse extends CreateFormRequest {
  id: number
  createdAt: string
  updatedAt: string
}

export type FormSection = 
  | 'cadastral' 
  | 'family' 
  | 'education' 
  | 'needs' 
  | 'professional' 
  | 'social' 
  | 'income' 
  | 'housing'

export interface FormState {
  data: CreateFormRequest
  isDirty: boolean
  isValid: boolean
  currentSection: FormSection
  expandedSections: FormSection[]
  isLoading: boolean
  lastSavedAt: string | null
}

export const FORM_SECTIONS: Record<FormSection, { id: FormSection; label: string; icon: string; order: number }> = {
  cadastral: { id: 'cadastral', label: 'Dados Cadastrais', icon: '📋', order: 0 },
  family: { id: 'family', label: 'Membros da Família', icon: '👨‍👩‍👧‍👦', order: 1 },
  education: { id: 'education', label: 'Dados Educacionais', icon: '📚', order: 2 },
  needs: { id: 'needs', label: 'Necessidades da Família', icon: '🤝', order: 3 },
  professional: { id: 'professional', label: 'Habilidades Profissionais', icon: '💼', order: 4 },
  social: { id: 'social', label: 'Problemas Sociais', icon: '❗', order: 5 },
  income: { id: 'income', label: 'Renda e Despesas', icon: '💰', order: 6 },
  housing: { id: 'housing', label: 'Situação Habitacional', icon: '🏠', order: 7 }
}

export const INITIAL_FORM_STATE: CreateFormRequest = {
  cadastralData: {
    applicantName: '',
    cpf: '',
    rg: '',
    birthDate: '',
    maritalStatus: '',
    address: '',
    number: '',
    neighborhood: '',
    city: '',
    state: '',
    cep: '',
    phone: '',
    email: '',
    familyMembersCount: 0
  },
  familyMembers: [],
  educationData: {
    schoolingLevel: '',
    isAttendingSchool: false,
    course: '',
    hasProfessionalTraining: false,
    trainingArea: ''
  },
  familyNeeds: [],
  essentialItems: [],
  healthNeeds: '',
  hasProfessionalSkills: false,
  occupation: '',
  experienceTime: '',
  occupationOther: '',
  skills: '',
  socialIssues: [],
  otherIssues: '',
  incomeExpensesData: {
    monthlyFamilyIncome: 0,
    totalExpenses: 0,
    rentValue: 0,
    receivesGovernmentBenefit: false,
    benefitType: '',
    benefitValue: 0
  },
  housingData: {
    housingType: '',
    ownProperty: false,
    roomsCount: 0,
    bedroomsCount: 0,
    hasBasicSanitation: false,
    hasElectricity: false,
    hasRunningWater: false
  }
}