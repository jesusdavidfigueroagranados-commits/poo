using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Colombia.Ingenieria.Unidad2.Ejercicio1
{
    public class ItemFactura
    {
        public string Descripcion { get; }
        public decimal PrecioUnitarioCOP { get; }
        public int Cantidad { get; }
        public bool AplicaIVA { get; }

        public ItemFactura(string desc, decimal precio, int cant, bool aplicaIva)
        {
            if (precio < 0 || cant <= 0)
                throw new ArgumentException("Precio o cantidad no válidos.");
            Descripcion = desc;
            PrecioUnitarioCOP = precio;
            Cantidad = cant;
            AplicaIVA = aplicaIva;
        }

        public decimal CalcularSubtotalItem() => PrecioUnitarioCOP * Cantidad;
        public decimal CalcularIVAItem() => AplicaIVA ? CalcularSubtotalItem() * 0.19m : 0.00m;
    }

    public class FacturaElectronicaDIAN
    {
        private readonly List<ItemFactura> _items = new();
        public string NITEmisor { get; }
        public string NITAdquiriente { get; }
        public string NumeroFactura { get; }

        public FacturaElectronicaDIAN(string nitEmisor, string nitAdquiriente, string numFactura)
        {
            NITEmisor = nitEmisor;
            NITAdquiriente = nitAdquiriente;
            NumeroFactura = numFactura;
        }

        public void AgregarItem(ItemFactura item) => _items.Add(item);

        public decimal CalcularSubtotal()
        {
            decimal sub = 0m;
            foreach (var it in _items) sub += it.CalcularSubtotalItem();
            return sub;
        }

        public decimal CalcularTotalIVA()
        {
            decimal iva = 0m;
            foreach (var it in _items) iva += it.CalcularIVAItem();
            return iva;
        }

        public string GenerarCufe()
        {
            string rawCadena = $"{NumeroFactura}{NITAdquiriente}{CalcularSubtotal():F2}";
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawCadena));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public void ImprimirFacturaDIAN()
        {
            decimal sub = CalcularSubtotal();
            decimal iva = CalcularTotalIVA();
            decimal reteFuente = sub * 0.025m;
            decimal totalPagar = (sub + iva) - reteFuente;

            Console.WriteLine($"===== FACTURA ELECTRONICA DIAN: {NumeroFactura} =====");
            Console.WriteLine($"NIT EMISOR      : {NITEmisor}");
            Console.WriteLine($"NIT ADQUIRIENTE : {NITAdquiriente}");
            Console.WriteLine($"CUFE            : {GenerarCufe()}");
            Console.WriteLine("------------------------------------------------");
            foreach (var it in _items)
            {
                Console.WriteLine($"{it.Descripcion,-35} x{it.Cantidad}  {it.CalcularSubtotalItem(),12:N2} COP");
            }
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"SUBTOTAL        : {sub,14:N2} COP");
            Console.WriteLine($"IVA (19%)       : {iva,14:N2} COP");
            Console.WriteLine($"RETEFUENTE 2.5% : -{reteFuente,13:N2} COP");
            Console.WriteLine($"TOTAL A PAGAR   : {totalPagar,14:N2} COP\n");
        }
    }

    class Program
    {
        static void Main()
        {
            var factura = new FacturaElectronicaDIAN("900123456", "800123456", "SETP99001024");

            // 10 ITEMS NUEVOS - DIFERENTES
            factura.AgregarItem(new ItemFactura("Servidor Dell PowerEdge", 4500000, 1, true));
            factura.AgregarItem(new ItemFactura("Licencia SQL Server 2022", 3200000, 1, true));
            factura.AgregarItem(new ItemFactura("Firewall Fortinet 60F", 1850000, 1, true));
            factura.AgregarItem(new ItemFactura("Cableado Estructurado Cat6", 25000, 20, true));
            factura.AgregarItem(new ItemFactura("Mantenimiento Preventivo", 180000, 4, false));
            factura.AgregarItem(new ItemFactura("Auditoria de Seguridad", 2200000, 1, false));
            factura.AgregarItem(new ItemFactura("Backup en la Nube 1TB", 450000, 2, true));
            factura.AgregarItem(new ItemFactura("Mouse y Teclado Logitech", 120000, 5, true));
            factura.AgregarItem(new ItemFactura("Instalacion de Camaras", 950000, 1, true));
            factura.AgregarItem(new ItemFactura("Curso Ciberseguridad", 750000, 3, false));

            factura.ImprimirFacturaDIAN();
        }
    }

}
