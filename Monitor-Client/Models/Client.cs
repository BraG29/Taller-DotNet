namespace Monitor_Client.Models
{
    public class Client{
        public Client()
        {

        }
        public Client(int CI, int Ap){
            this.Ci = CI;
            this.AtenttionPlace = Ap;
        }

        public int Ci { get; set; }
        public int AtenttionPlace {  get; set; }
    }
}
