<script setup>
defineProps({
  title: {
    type: String,
    required: true
  },
  icon: {
    type: String,
    default: ''
  },
  isExpanded: {
    type: Boolean,
    default: false
  }
})

defineEmits(['toggle'])
</script>

<template>
  <div class="expansion-panel" :class="{ expanded: isExpanded }">
    <button class="panel-header" @click="$emit('toggle')">
      <h3>{{ icon }} {{ title }}</h3>
    </button>
    <div v-show="isExpanded" class="panel-content">
      <slot />
    </div>
  </div>
</template>

<style scoped>
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
  margin: 0;
}

.panel-content {
  padding: 1.5rem;
  background: white;
}
</style>