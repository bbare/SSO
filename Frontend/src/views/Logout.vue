
<script>
export default {
    name: 'Logout',
    created()
    {
      localStorage.removeItem('token'),
      this.$router.push('/home')
      
    }
    
}
</script>
