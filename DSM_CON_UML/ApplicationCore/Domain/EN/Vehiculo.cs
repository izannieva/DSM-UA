namespace ApplicationCore.Domain.EN
{
    public class Vehiculo
    {
        public virtual long IdVehiculo { get; set; }
        public virtual string Marca { get; set; } = string.Empty;
        public virtual string Modelo { get; set; } = string.Empty;
        public virtual int Anio { get; set; }
        public virtual int Kilometros { get; set; }
        public virtual string? Descripcion { get; set; }
        public virtual decimal PrecioBase { get; set; }

        // FK
        public virtual long CategoriaId { get; set; }

        // Navigation
        public virtual Categoria? Categoria { get; set; }
        public virtual IList<Anuncio>? Anuncios { get; set; }
    }
}
