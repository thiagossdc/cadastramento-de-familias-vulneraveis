<script setup>
const props = defineProps({
  modelValue: {
    type: Object,
    required: true
  },
  maritalStatusOptions: {
    type: Array,
    required: true
  }
})

const emit = defineEmits(['update:modelValue'])

const updateField = (field, value) => {
  emit('update:modelValue', {
    ...props.modelValue,
    [field]: value
  })
}
</script>

<template>
  <div class="section-content">
    <div class="form-row">
      <div class="form-group">
        <label>Nome Completo do Solicitante *</label>
        <input v-model="cadastralData.applicantName" type="text" required />
      </div>
      <div class="form-group half-width">
        <label>CPF *</label>
        <input v-model="cadastralData.cpf" type="text" required />
      </div>
      <div class="form-group half-width">
        <label>RG</label>
        <input v-model="cadastralData.rg" type="text" />
      </div>
    </div>

    <div class="form-row">
      <div class="form-group third-width">
        <label>Data de Nascimento *</label>
        <input v-model="cadastralData.birthDate" type="date" required />
      </div>
      <div class="form-group third-width">
        <label>Estado Civil</label>
        <select v-model="cadastralData.maritalStatus">
          <option value="">Selecione</option>
          <option v-for="opt in maritalStatusOptions" :key="opt" :value="opt">{{ opt }}</option>
        </select>
      </div>
      <div class="form-group third-width">
        <label>Quantidade de Membros na Família</label>
        <input v-model.number="cadastralData.familyMembersCount" type="number" min="1" />
      </div>
    </div>

    <div class="form-row">
      <div class="form-group">
        <label>Endereço *</label>
        <input v-model="cadastralData.address" type="text" required />
      </div>
      <div class="form-group small-width">
        <label>Número</label>
        <input v-model="cadastralData.number" type="text" />
      </div>
      <div class="form-group">
        <label>Bairro *</label>
        <input v-model="cadastralData.neighborhood" type="text" required />
      </div>
    </div>

    <div class="form-row">
      <div class="form-group half-width">
        <label>Cidade *</label>
        <input v-model="cadastralData.city" type="text" required />
      </div>
      <div class="form-group small-width">
        <label>UF *</label>
        <select v-model="cadastralData.state" required>
          <option value="">Selecione</option>
          <option value="GO">Goiás</option>
          <option value="DF">Distrito Federal</option>
          <option value="MG">Minas Gerais</option>
          <option value="SP">São Paulo</option>
        </select>
      </div>
      <div class="form-group half-width">
        <label>CEP</label>
        <input v-model="cadastralData.cep" type="text" />
      </div>
    </div>

    <div class="form-row">
      <div class="form-group half-width">
        <label>Telefone *</label>
        <input v-model="cadastralData.phone" type="text" required />
      </div>
      <div class="form-group half-width">
        <label>E-mail</label>
        <input v-model="cadastralData.email" type="email" />
      </div>
    </div>
  </div>
</template>

<style scoped>
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
.form-group select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.2s, box-shadow 0.2s;
}

.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}
</style>