<script setup>
const props = defineProps({
  member: {
    type: Object,
    required: true
  },
  index: {
    type: Number,
    required: true
  },
  relationshipOptions: {
    type: Array,
    required: true
  },
  schoolingOptions: {
    type: Array,
    required: true
  }
})

defineEmits(['remove'])
</script>

<template>
  <div class="family-member">
    <div class="member-header">
      <h4>Membro {{ index + 1 }}</h4>
      <button type="button" class="btn-delete" @click="$emit('remove', index)">✕</button>
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
</template>

<style scoped>
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
  margin: 0;
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