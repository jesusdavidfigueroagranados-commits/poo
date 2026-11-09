using System;

namespace TallerHotel
{
    public class HabitacionHotel
    {
        // 1. ATRIBUTOS PRIVADOS

        private int numero;
        private string tipo;
        private double precioPorNoche;
        private bool disponible;
        private string clienteActual;

        // 2. PROPIEDADES PÚBLICAS

        public int Numero
        {
            get { return numero; }
        }

        public string Tipo
        {
            get { return tipo; }
            set
            {
                if (value == "Individual" || value == "Doble" || value == "Suite")
                {
                    tipo = value;
                }
                else
                {
                    Console.WriteLine($"Error: Tipo '{value}' no válido. Solo se permite Individual, Doble o Suite.");
                }
            }
        }

        public double PrecioPorNoche
        {
            get { return precioPorNoche; }
            set
            {
                if (value > 0)
                {
                    precioPorNoche = value;
                }
                else
                {
                    Console.WriteLine("Error: El precio por noche debe ser mayor a 0.");
                }
            }
        }

        public bool Disponible
        {
            get { return disponible; }
        }

        public string ClienteActual
        {
            get { return clienteActual; }
        }

        // 3. CONSTRUCTOR

        public HabitacionHotel(int numero, string tipo, double precioPorNoche)
        {
            this.numero = numero;
            this.Tipo = tipo; // Usa la propiedad con validación
            this.PrecioPorNoche = precioPorNoche; // Usa la propiedad con validación
            this.disponible = true;
            this.clienteActual = "";
        }

        // 4. MÉTODOS CON LÓGICA

        public double Reservar(string nombreCliente, int noches)
        {
            if (disponible)
            {
                disponible = false;
                clienteActual = nombreCliente;
                double costoTotal = precioPorNoche * noches;
                return costoTotal;
            }
            else
            {
                return -1;
            }
        }

        public bool Liberar()
        {
            if (!disponible) // Si está ocupada
            {
                disponible = true;
                clienteActual = "";
                return true;
            }
            else // Si ya estaba libre
            {
                return false;
            }
        }

        public string MostrarInfo()
        {
            string estado = disponible ? "Disponible" : $"Ocupada por {clienteActual}";
            return $"Habitación {numero} - Tipo: {tipo} - Precio: ${precioPorNoche:N0} por noche - Estado: {estado}";
        }

        // NUEVO MÉTODO A: Aplicar Descuento con validación de precio mínimo
        public void AplicarDescuento(int porcentaje)
        {
            if (porcentaje < 0 || porcentaje > 100) return;

            double descuento = precioPorNoche * (porcentaje / 100.0);
            double nuevoPrecio = precioPorNoche - descuento;

            // Aseguramos que el precio resultante nunca sea menor o igual a 0 (mínimo dejamos un valor simbólico o 1, o evitamos que baje de 0)
            if (nuevoPrecio <= 0)
            {
                Console.WriteLine($"Aviso: El descuento aplicado a la habitación {numero} dejaría el precio en 0 o negativo. Se ajusta al precio mínimo de $1.");
                precioPorNoche = 1; 
            }
            else
            {
                precioPorNoche = nuevoPrecio;
            }
        }

        // NUEVO MÉTODO B: Obtener Datos Resumen en formato corto
        public string ObtenerDatosResumen()
        {
            string estado = disponible ? "Disponible" : $"Ocupada por: {clienteActual}";
            return $"[{numero}] {Tipo} - ${precioPorNoche:N0} - {estado}";
        }
    }

    // PROGRAMA PRINCIPAL (Main)
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Instanciar al menos 3 habitaciones

            HabitacionHotel hab101 = new HabitacionHotel(101, "Individual", 50000);
            HabitacionHotel hab202 = new HabitacionHotel(202, "Doble", 80000);
            HabitacionHotel hab303 = new HabitacionHotel(303, "Suite", 150000);

            HabitacionHotel[] habitaciones = { hab101, hab202, hab303 };

            // 2. Mostrar estado inicial usando el resumen o info detallada
            Console.WriteLine("=== ESTADO INICIAL (RESUMEN) ===");
            foreach (var h in habitaciones)
            {
                Console.WriteLine(h.ObtenerDatosResumen());
            }

            Console.WriteLine("\n=== SIMULANDO ACCIONES ===");

            // 3. Simular acciones
            double costo = hab101.Reservar("Juana Pérez", 3);
            Console.WriteLine($"Reserva 101 para Juana Pérez por 3 noches -> Costo total: ${costo:N0}");

            costo = hab202.Reservar("Luis Gómez", 2);
            Console.WriteLine($"Reserva 202 para Luis Gómez por 2 noches -> Costo total: ${costo:N0}");

            hab303.AplicarDescuento(10);
            Console.WriteLine($"Aplicar descuento del 10% a la 303 -> Nuevo precio: ${hab303.PrecioPorNoche:N0}");

            // 4. Menú básico / Bucle de control y métricas solicitadas en el punto 2 del taller
            Console.WriteLine("\n=== ESTADO FINAL (RESUMEN) ===");
            foreach (var h in habitaciones)
            {
                Console.WriteLine(h.ObtenerDatosResumen());
            }

            // A. Contar y mostrar cuántas habitaciones quedaron libres y cuántas ocupadas usando un ciclo foreach
            int libres = 0;
            int ocupadas = 0;
            foreach (var h in habitaciones)
            {
                if (h.Disponible)
                {
                    libres++;
                }
                else
                {
                    ocupadas++;
                }
            }

            // B. Calcular el precio promedio de todas las habitaciones creadas en el hotel
            double sumaPrecios = 0;
            foreach (var h in habitaciones)
            {
                sumaPrecios += h.PrecioPorNoche;
            }
            double precioPromedio = sumaPrecios / habitaciones.Length;

            // Mostrar métricas finales
            Console.WriteLine("\n>>> MÉTRICAS FINALES DEL HOTEL <<<");
            Console.WriteLine($"- Habitaciones libres: {libres}");
            Console.WriteLine($"- Habitaciones ocupadas: {ocupadas}");
            Console.WriteLine($"- Precio promedio por noche: ${precioPromedio:N2}");

            Console.ReadKey();
        }
    }
}