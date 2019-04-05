
<script>
import axios from "axios"
import { apiURL } from "@/const.js"
import { store } from '@/services/request'
export default {
  name: "Logout",
  data() {
    return {
      
      token: ""
    }
  },
  created() {
    
      const url = `${apiURL}/Logout`
    
      axios.post(url, 
        {
          token: this.$data.token
        })
        .then(resp=>{
          let respData = resp.data;
          alert(respData);
          localStorage.removeItem('token');
          store.state.isLogin = false;
          store.getEmail();
          this.$router.push("/home");
         
         
        })

        .catch(e => {
          if (e.response.status === 417) {
            alert("Session has not been deleted");
            this.$router.push("/dashboard");
          }
          
        });
    
  }
};
</script>
