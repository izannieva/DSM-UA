namespace ApplicationCore.Domain.EN
{
    public class Anuncio
    {
        public virtual long IdAnuncio { get; set; }
        public virtual string Titulo { get; set; } = string.Empty;
        public virtual System.DateTime FechaPublicacion { get; set; }
        public virtual string Estado { get; set; } = string.Empty;
        public virtual decimal PrecioVenta { get; set; }

        // FKs
        public virtual long UsuarioId { get; set; }
        public virtual long VehiculoId { get; set; }

        // Navigation
        public virtual Usuario? Usuario { get; set; }
        public virtual Vehiculo? Vehiculo { get; set; }
        public virtual IList<Mensaje>? Mensajes { get; set; }
        public virtual IList<Pago>? Pagos { get; set; }
    }
}
