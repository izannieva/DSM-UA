namespace ApplicationCore.Domain.EN
{
    public class Categoria
    {
        public virtual long IdCategoria { get; set; }
        public virtual string Nombre { get; set; } = string.Empty;

        public virtual IList<Vehiculo>? Vehiculos { get; set; }
    }
}
