<script setup>
import { ref, reactive } from 'vue'

const form = reactive({
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
})

const expandedPanel = ref(0)
const loading = ref(false)
const message = ref('')
const messageType = ref('success')

const expandPanel = (index) => {
  expandedPanel.value = expandedPanel.value === index ? -1 : index
}

const addFamilyMember = () => {
  form.familyMembers.push({
    name: '',
    age: 0,
    relationship: '',
    occupation: '',
    schooling: ''
  })
}

const removeFamilyMember = (index) => {
  form.familyMembers.splice(index, 1)
}

const toggleCheckbox = (array, value) => {
  const index = array.indexOf(value)
  if (index === -1) {
    array.push(value)
  } else {
    array.splice(index, 1)
  }
}

const submitForm = async () => {
  loading.value = true
  try {
    const response = await fetch('http://localhost:5108/api/forms', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(form)
    })

    if (response.ok) {
      const result = await response.json()
      message.value = 'Cadastro realizado com sucesso! ID: ' + result.id
      messageType.value = 'success'
      resetForm()
    } else {
      const error = await response.json()
      message.value = 'Erro ao cadastrar: ' + (error.title || 'Erro desconhecido')
      messageType.value = 'error'
    }
  } catch (err) {
    message.value = 'Erro de conexão com a API. Verifique se o servidor está rodando.'
    messageType.value = 'error'
  } finally {
    loading.value = false
    setTimeout(() => message.value = '', 5000)
  }
}

const resetForm = () => {
  Object.assign(form, {
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
  })
  expandedPanel.value = 0
}

const maritalStatusOptions = ['Solteiro(a)', 'Casado(a)', 'Divorciado(a)', 'Viúvo(a)', 'União Estável']
const relationshipOptions = ['Filho(a)', 'Esposa', 'Esposo', 'Mãe', 'Pai', 'Irmão(ã)', 'Outro']
const schoolingOptions = ['Analfabeto', 'Fundamental Incompleto', 'Fundamental Completo', 'Médio Incompleto', 'Médio Completo', 'Superior Incompleto', 'Superior Completo', 'Pós-graduação']
const housingTypeOptions = ['Casa', 'Apartamento', 'Barraco', 'Ocupaçao', 'Aluguel', 'Cedido', 'Outro']

const familyNeedsOptions = [
  'Alimentação', 'Roupas', 'Móveis', 'Eletrodomésticos', 'Material Escolar',
  'Transporte', 'Medicamentos', 'Assistência Médica', 'Acompanhamento Psicológico',
  'Auxílio Moradia', 'Vaga em Creche', 'Orientação Jurídica'
]

const essentialItemsOptions = [
  'Cesta Básica', 'Colchão', 'Cobertor', 'Roupa de Cama', 'Mesa', 'Cadeira',
  'Armário', 'Fogão', 'Geladeira', 'Máquina de Lavar', 'Ventilador', 'TV'
]

const socialIssuesOptions = [
  'Desemprego', 'Baixa Renda', 'Falta de Moradia', 'Violência Doméstica',
  'Dependência Química', 'Doenças Crônicas', 'Deficiência', 'Analfabetismo',
  'Isolamento Social', 'Trabalho Infantil', 'Exploração Sexual', 'Tráfico de Pessoas'
]
</script>

<template>
  <div class="app-container">
    <header class="app-header">
      <h1>Cadastro de Famílias em Situação de Vulnerabilidade</h1>
    </header>

    <main class="main-content">
      <div class="card intro-card">
        <h2>Formulário de Cadastro Social</h2>
        <p>Preencha todas as informações solicitadas abaixo para cadastrar uma família em situação de vulnerabilidade social.</p>
      </div>

      <div v-if="message" class="snackbar" :class="messageType">
        {{ message }}
      </div>

      <div class="form-container">
        <!-- Panels de seções -->
        <div class="expansion-panel" :class="{ expanded: expandedPanel === 0 }">
          <button class="panel-header" @click="expandPanel(0)">
            <h3>📋 Dados Cadastrais</h3>
          </button>
          <div v-show="expandedPanel === 0" class="panel-content">
            <div class="form-row">
              <div class="form-group">
                <label>Nome Completo do Solicitante *</label>
                <input v-model="form.cadastralData.applicantName" type="text" required />
              </div>
              <div class="form-group half-width">
                <label>CPF *</label>
                <input v-model="form.cadastralData.cpf" type="text" required />
              </div>
              <div class="form-group half-width">
                <label>RG</label>
                <input v-model="form.cadastralData.rg" type="text" />
              </div>
            </div>

            <div class="form-row">
              <div class="form-group third-width">
                <label>Data de Nascimento *</label>
                <input v-model="form.cadastralData.birthDate" type="date" required />
              </div>
              <div class="form-group third-width">
                <label>Estado Civil</label>
                <select v-model="form.cadastralData.maritalStatus">
                  <option value="">Selecione</option>
                  <option v-for="opt in maritalStatusOptions" :key="opt" :value="opt">{{ opt }}</option>
                </select>
              </div>
              <div class="form-group third-width">
                <label>Quantidade de Membros na Família</label>
                <input v-model.number="form.cadastralData.familyMembersCount" type="number" min="1" />
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label>Endereço *</label>
                <input v-model="form.cadastralData.address" type="text" required />
              </div>
              <div class="form-group small-width">
                <label>Número</label>
                <input v-model="form.cadastralData.number" type="text" />
              </div>
              <div class="form-group">
                <label>Bairro *</label>
                <input v-model="form.cadastralData.neighborhood" type="text" required />
              </div>
            </div>

            <div class="form-row">
              <div class="form-group half-width">
                <label>Cidade *</label>
                <input v-model="form.cadastralData.city" type="text" required />
              </div>
              <div class="form-group small-width">
                <label>UF *</label>
                <select v-model="form.cadastralData.state" required>
                  <option value="">Selecione</option>
                  <option value="GO">Goiás</option>
                  <option value="DF">Distrito Federal</option>
                  <option value="MG">Minas Gerais</option>
                  <option value="SP">São Paulo</option>
                </select>
              </div>
              <div class="form-group half-width">
                <label>CEP</label>
                <input v-model="form.cadastralData.cep" type="text" />
              </div>
            </div>

            <div class="form-row">
              <div class="form-group half-width">
                <label>Telefone *</label>
                <input v-model="form.cadastralData.phone" type="text" required />
              </div>
              <div class="form-group half-width">
                <label>E-mail</label>
                <input v-model="form.cadastralData.email" type="email" />
              </div>
            </div>
          </div>
        </div>

        <div class="expansion-panel" :class="{ expanded: expandedPanel === 1 }">
          <button class="panel-header" @click="expandPanel(1)">
            <h3>👨‍👩‍👧‍👦 Membros da Família</h3>
          </button>
          <div v-show="expandedPanel === 1" class="panel-content">
            <div v-for="(member, index) in form.familyMembers" :key="index" class="family-member">
              <div class="member-header">
                <h4>Membro {{ index + 1 }}</h4>
                <button type="button" class="btn-delete" @click="removeFamilyMember(index)">✕</button>
              </div>
              
              <div class="form-row">
                <div class="form-group">
                  <label>Nome *</label>
                  <input v-model="member.name" type="text" required />
                </div>
                <div class="form-group third-width">
                  <label>Idade *</label>
                  <input v-model.number="member.age" type="number" min="0" required />
                </div>
                <div class="form-group">
                  <label>Parentesco *</label>
                  <select v-model="member.relationship" required>
                    <option value="">Selecione</option>
                    <option v-for="opt in relationshipOptions" :key="opt" :value="opt">{{ opt }}</option>
                  </select>
                </div>
              </div>

              <div class="form-row">
                <div class="form-group half-width">
                  <label>Ocupação</label>
                  <input v-model="member.occupation" type="text" />
                </div>
                <div class="form-group half-width">
                  <label>Escolaridade</label>
                  <select v-model="member.schooling">
                    <option value="">Selecione</option>
                    <option v-for="opt in schoolingOptions" :key="opt" :value="opt">{{ opt }}</option>
                  </select>
                </div>
              </div>
            </div>

            <button type="button" class="btn btn-secondary" @click="addFamilyMember">
              ➕ Adicionar Membro
            </button>
          </div>
        </div>

        <div class="expansion-panel" :class="{ expanded: expandedPanel === 2 }">
          <button class="panel-header" @click="expandPanel(2)">
            <h3>📚 Dados Educacionais</h3>
          </button>
          <div v-show="expandedPanel === 2" class="panel-content">
            <div class="form-row">
              <div class="form-group half-width">
                <label>Nível de Escolaridade</label>
                <select v-model="form.educationData.schoolingLevel">
                  <option value="">Selecione</option>
                  <option v-for="opt in schoolingOptions" :key="opt" :value="opt">{{ opt }}</option>
                </select>
              </div>
              <div class="form-group half-width checkbox-item">
                <input v-model="form.educationData.isAttendingSchool" type="checkbox" id="attendingSchool" />
                <label for="attendingSchool">Atualmente estudando</label>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group half-width">
                <label>Curso / Série</label>
                <input v-model="form.educationData.course" type="text" />
              </div>
              <div class="form-group half-width checkbox-item">
                <input v-model="form.educationData.hasProfessionalTraining" type="checkbox" id="hasTraining" />
                <label for="hasTraining">Possui Curso Profissionalizante</label>
              </div>
            </div>

            <div class="form-row" v-if="form.educationData.hasProfessionalTraining">
              <div class="form-group">
                <label>Área de Capacitação</label>
                <input v-model="form.educationData.trainingArea" type="text" />
              </div>
            </div>
          </div>
        </div>

        <div class="expansion-panel" :class="{ expanded: expandedPanel === 3 }">
          <button class="panel-header" @click="expandPanel(3)">
            <h3>🤝 Necessidades da Família</h3>
          </button>
          <div v-show="expandedPanel === 3" class="panel-content">
            <h4>Principais Necessidades</h4>
            <div class="checkbox-grid">
              <div v-for="item in familyNeedsOptions" :key="item" class="checkbox-item">
                <input :id="'need-' + item" type="checkbox" :checked="form.familyNeeds.includes(item)" @change="toggleCheckbox(form.familyNeeds, item)" />
                <label :for="'need-' + item">{{ item }}</label>
              </div>
            </div>

            <h4 style="margin-top: 2rem;">Itens Essenciais Necessários</h4>
            <div class="checkbox-grid">
              <div v-for="item in essentialItemsOptions" :key="item" class="checkbox-item">
                <input :id="'item-' + item" type="checkbox" :checked="form.essentialItems.includes(item)" @change="toggleCheckbox(form.essentialItems, item)" />
                <label :for="'item-' + item">{{ item }}</label>
              </div>
            </div>

            <div class="form-group" style="margin-top: 2rem;">
              <label>Necessidades de Saúde</label>
              <textarea v-model="form.healthNeeds" rows="4" placeholder="Descreva necessidades de saúde da família"></textarea>
            </div>
          </div>
        </div>

        <div class="expansion-panel" :class="{ expanded: expandedPanel === 4 }">
          <button class="panel-header" @click="expandPanel(4)">
            <h3>💼 Dados Profissionais</h3>
          </button>
          <div v-show="expandedPanel === 4" class="panel-content">
            <div class="form-row">
              <div class="form-group half-width checkbox-item">
                <input v-model="form.hasProfessionalSkills" type="checkbox" id="hasSkills" />
                <label for="hasSkills">Possui Habilidades Profissionais</label>
              </div>
            </div>
            
            <div class="form-row" v-if="form.hasProfessionalSkills">
              <div class="form-group">
                <label>Profissão / Ocupação</label>
                <input v-model="form.occupation" type="text" />
              </div>
              <div class="form-group third-width">
                <label>Tempo de Experiência</label>
                <input v-model="form.experienceTime" type="text" />
              </div>
            </div>

            <div class="form-group">
              <label>Descreva suas habilidades</label>
              <textarea v-model="form.skills" rows="3"></textarea>
            </div>
          </div>
        </div>

        <div class="expansion-panel" :class="{ expanded: expandedPanel === 5 }">
          <button class="panel-header" @click="expandPanel(5)">
            <h3>⚠️ Problemas Sociais</h3>
          </button>
          <div v-show="expandedPanel === 5" class="panel-content">
            <h4>Marque os problemas que afetam a família</h4>
            <div class="checkbox-grid">
              <div v-for="item in socialIssuesOptions" :key="item" class="checkbox-item">
                <input :id="'issue-' + item" type="checkbox" :checked="form.socialIssues.includes(item)" @change="toggleCheckbox(form.socialIssues, item)" />
                <label :for="'issue-' + item">{{ item }}</label>
              </div>
            </div>

            <div class="form-group" style="margin-top: 2rem;">
              <label>Outros problemas ou observações</label>
              <textarea v-model="form.otherIssues" rows="3"></textarea>
            </div>
          </div>
        </div>

        <div class="expansion-panel" :class="{ expanded: expandedPanel === 6 }">
          <button class="panel-header" @click="expandPanel(6)">
            <h3>💰 Renda e Despesas</h3>
          </button>
          <div v-show="expandedPanel === 6" class="panel-content">
            <div class="form-row">
              <div class="form-group half-width">
                <label>Renda Familiar Mensal Total (R$)</label>
                <input v-model.number="form.incomeExpensesData.monthlyFamilyIncome" type="number" min="0" />
              </div>
              <div class="form-group half-width">
                <label>Despesas Mensais Totais (R$)</label>
                <input v-model.number="form.incomeExpensesData.totalExpenses" type="number" min="0" />
              </div>
            </div>

            <div class="form-row">
              <div class="form-group half-width">
                <label>Valor do Aluguel (R$)</label>
                <input v-model.number="form.incomeExpensesData.rentValue" type="number" min="0" />
              </div>
              <div class="form-group half-width checkbox-item">
                <input v-model="form.incomeExpensesData.receivesGovernmentBenefit" type="checkbox" id="receivesBenefit" />
                <label for="receivesBenefit">Recebe Benefício Governamental</label>
              </div>
            </div>

            <div class="form-row" v-if="form.incomeExpensesData.receivesGovernmentBenefit">
              <div class="form-group half-width">
                <label>Tipo de Benefício</label>
                <input v-model="form.incomeExpensesData.benefitType" type="text" />
              </div>
              <div class="form-group half-width">
                <label>Valor do Benefício (R$)</label>
                <input v-model.number="form.incomeExpensesData.benefitValue" type="number" min="0" />
              </div>
            </div>
          </div>
        </div>

        <div class="expansion-panel" :class="{ expanded: expandedPanel === 7 }">
          <button class="panel-header" @click="expandPanel(7)">
            <h3>🏠 Dados da Moradia</h3>
          </button>
          <div v-show="expandedPanel === 7" class="panel-content">
            <div class="form-row">
              <div class="form-group half-width">
                <label>Tipo de Moradia</label>
                <select v-model="form.housingData.housingType">
                  <option value="">Selecione</option>
                  <option v-for="opt in housingTypeOptions" :key="opt" :value="opt">{{ opt }}</option>
                </select>
              </div>
              <div class="form-group half-width checkbox-item">
                <input v-model="form.housingData.ownProperty" type="checkbox" id="ownProperty" />
                <label for="ownProperty">Propriedade Própria</label>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group third-width">
                <label>Quantidade de Cômodos</label>
                <input v-model.number="form.housingData.roomsCount" type="number" min="0" />
              </div>
              <div class="form-group third-width">
                <label>Quantidade de Quartos</label>
                <input v-model.number="form.housingData.bedroomsCount" type="number" min="0" />
              </div>
            </div>

            <div style="margin-top: 1rem;">
              <div class="checkbox-item">
                <input v-model="form.housingData.hasBasicSanitation" type="checkbox" id="sanitation" />
                <label for="sanitation">Possui Saneamento Básico</label>
              </div>
              <div class="checkbox-item">
                <input v-model="form.housingData.hasElectricity" type="checkbox" id="electricity" />
                <label for="electricity">Possui Energia Elétrica</label>
              </div>
              <div class="checkbox-item">
                <input v-model="form.housingData.hasRunningWater" type="checkbox" id="water" />
                <label for="water">Possui Água Encanada</label>
              </div>
            </div>
          </div>
        </div>

        <div class="form-actions">
          <button type="button" class="btn btn-secondary" @click="resetForm">Limpar Formulário</button>
          <button type="button" class="btn btn-primary" @click="submitForm" :disabled="loading">
            <span v-if="loading">Enviando...</span>
            <span v-else>📤 Enviar Cadastro</span>
          </button>
        </div>
      </div>
    </main>

    <footer class="app-footer">
      <p>Sistema de Cadastro de Famílias © 2026</p>
    </footer>
  </div>
</template>

<style scoped>
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

.app-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding-bottom: 2rem;
}

.app-header {
  background: rgba(255, 255, 255, 0.95);
  padding: 1.5rem;
  text-align: center;
  box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}

.app-header h1 {
  color: #333;
  font-size: 1.8rem;
  font-weight: 600;
}

.main-content {
  max-width: 1000px;
  margin: 2rem auto;
  padding: 0 1rem;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.intro-card {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  margin-bottom: 2rem;
  box-shadow: 0 4px 15px rgba(0,0,0,0.1);
  width: 100%;
  text-align: center;
}

.intro-card h2 {
  color: #333;
  margin-bottom: 0.5rem;
  font-size: 1.4rem;
}

.intro-card p {
  color: #666;
  line-height: 1.6;
}

.snackbar {
  position: fixed;
  bottom: 2rem;
  left: 50%;
  transform: translateX(-50%);
  padding: 1rem 2rem;
  border-radius: 8px;
  color: white;
  font-weight: 500;
  z-index: 1000;
  animation: slideUp 0.3s ease;
}

.snackbar.success {
  background: #10b981;
}

.snackbar.error {
  background: #ef4444;
}

.form-container {
  background: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 4px 15px rgba(0,0,0,0.1);
  width: 100%;
}

.expansion-panel {
  border-bottom: 1px solid #eee;
}

.panel-header {
  width: 100%;
  background: #f8fafc;
  border: none;
  padding: 1.2rem 1.5rem;
  text-align: left;
  cursor: pointer;
  transition: background 0.2s;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.panel-header:hover {
  background: #f1f5f9;
}

.panel-header h3 {
  color: #333;
  font-size: 1.1rem;
  font-weight: 600;
}

.panel-content {
  padding: 1.5rem;
  background: white;
}

.form-row {
  display: flex;
  gap: 1rem;
  margin-bottom: 1rem;
  flex-wrap: wrap;
}

.form-group {
  flex: 1;
  min-width: 200px;
}

.form-group.half-width {
  flex: 0.5;
  min-width: 150px;
}

.form-group.third-width {
  flex: 0.33;
  min-width: 120px;
}

.form-group.small-width {
  flex: 0.25;
  min-width: 80px;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #374151;
  font-weight: 500;
  font-size: 0.9rem;
}

.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.2s, box-shadow 0.2s;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.checkbox-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 0.75rem;
  margin-top: 0.5rem;
}

.checkbox-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.checkbox-item input[type="checkbox"] {
  width: auto;
  width: 18px;
  height: 18px;
  cursor: pointer;
}

.checkbox-item label {
  margin: 0;
  cursor: pointer;
  font-weight: normal;
}

.family-member {
  background: #f8fafc;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1rem;
}

.member-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.member-header h4 {
  color: #374151;
  font-size: 1rem;
}

.btn-delete {
  background: #ef4444;
  color: white;
  border: none;
  width: 28px;
  height: 28px;
  border-radius: 50%;
  cursor: pointer;
  font-weight: bold;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.2s;
}

.btn-delete:hover {
  background: #dc2626;
}

.btn {
  padding: 0.875rem 1.5rem;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.2s, transform 0.1s;
}

.btn:active {
  transform: scale(0.98);
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover {
  opacity: 0.9;
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-secondary {
  background: #e5e7eb;
  color: #374151;
}

.btn-secondary:hover {
  background: #d1d5db;
}

.form-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  padding: 1.5rem;
  background: #f8fafc;
}

.app-footer {
  text-align: center;
  padding: 1.5rem;
  color: rgba(255,255,255,0.8);
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateX(-50%) translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) translateY(0);
  }
}

@media (max-width: 768px) {
  .form-row {
    flex-direction: column;
  }
  
  .form-group,
  .form-group.half-width,
  .form-group.third-width,
  .form-group.small-width {
    flex: 1;
    min-width: 100%;
  }

  .checkbox-grid {
    grid-template-columns: 1fr;
  }

  .form-actions {
    flex-direction: column;
  }

  .app-header h1 {
    font-size: 1.4rem;
  }
}
</style>
