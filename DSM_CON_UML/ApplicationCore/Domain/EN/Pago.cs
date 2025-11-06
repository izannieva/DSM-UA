namespace ApplicationCore.Domain.EN
{
    public class Pago
    {
        public virtual long IdPago { get; set; }
        public virtual System.DateTime FechaPago { get; set; }
        public virtual decimal Cantidad { get; set; }
        public virtual string TipoPago { get; set; } = string.Empty;

        // FKs
        public virtual long UsuarioId { get; set; }
        public virtual long AnuncioId { get; set; }

        // Navigation
        public virtual Usuario? Usuario { get; set; }
        public virtual Anuncio? Anuncio { get; set; }
    }
}
