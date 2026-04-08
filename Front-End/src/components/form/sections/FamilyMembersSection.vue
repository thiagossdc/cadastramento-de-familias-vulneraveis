<script setup>
import FamilyMember from '../FamilyMember.vue'

const props = defineProps({
  familyMembers: {
    type: Array,
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

defineEmits(['add', 'remove'])
</script>

<template>
  <div class="section-content">
    <FamilyMember
      v-for="(member, index) in familyMembers"
      :key="index"
      :member="member"
      :index="index"
      :relationship-options="relationshipOptions"
      :schooling-options="schoolingOptions"
      :can-remove="familyMembers.length > 1"
      @remove="$emit('remove', index)"
    />

    <button type="button" class="btn btn-secondary" @click="$emit('add')">
      ➕ Adicionar Membro
    </button>
  </div>
</template>

<style scoped>
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

.btn-secondary {
  background: #e5e7eb;
  color: #374151;
}

.btn-secondary:hover {
  background: #d1d5db;
}
</style>