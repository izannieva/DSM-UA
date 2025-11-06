namespace ApplicationCore.Domain.EN
{
    public class Usuario
    {
        public virtual long IdUsuario { get; set; }
        public virtual string Nombre { get; set; } = string.Empty;
        public virtual string Email { get; set; } = string.Empty;
        public virtual string Password { get; set; } = string.Empty;
        public virtual string? Telefono { get; set; }

        // Navigation
        public virtual IList<Anuncio>? Anuncios { get; set; }
        public virtual IList<Mensaje>? Mensajes { get; set; }
        public virtual IList<Pago>? Pagos { get; set; }
    }
}
