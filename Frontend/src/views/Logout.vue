
<script>
export default {
    methods:{
    logout(){
      localStorage.removeItem(localStorage.token),
      this.$router.push('/home')

    }
  }
}
</script>
