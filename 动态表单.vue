<template>
    <form @submit.prevent="submitForm">
      <div v-for="field in fields" :key="field.id">
        <label :for="field.id">{{ field.label }}</label>
        <input :type="field.type" :id="field.id" :name="field.name" v-model="form[field.name]" :required="field.required">
      </div>
      <button type="submit">提交</button>
    </form>
  </template>
   
  <script>
  export default {
    data() {
      return {
        form: {
          username: '',
          password: '',
          email: ''
        },
        fields: [
          { id: 'username', name: 'username', label: '用户名', type: 'text', required: true },
          { id: 'password', name: 'password', label: '密码', type: 'password', required: true },
          { id: 'email', name: 'email', label: '邮箱', type: 'email', required: false }
        ]
      }
    },
    methods: {
      submitForm() {
        // 提交表单
        console.log(this.form)
      }
    }
  }
  </script>