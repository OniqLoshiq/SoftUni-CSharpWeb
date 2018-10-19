namespace SIS.MVCFramework.Models
{
    public class Model
    {
        private bool? isValid;

        public bool? Isvalid
        {
            get => this.isValid;
            set => this.isValid = this.isValid ?? value;
        }
    }
}
