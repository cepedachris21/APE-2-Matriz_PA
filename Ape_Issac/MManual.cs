using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ape_Issac
{
    public partial class MManual : Form
    {
        public MManual()
        {
            InitializeComponent();

            // Configurar los DataGridView al inicio
            ConfigurarPropiedadesDataGridView(dataGridView_M1);
            ConfigurarPropiedadesDataGridView(dataGridView_M2);
            ConfigurarPropiedadesDataGridView(dataGridView_M3);
            ConfigurarPropiedadesDataGridView(dataGridView_Traspuesta);

            // El DataGridView M3 y Traspuesta nunca deben ser editables ya que son para resultados
            dataGridView_M3.ReadOnly = true;
            dataGridView_Traspuesta.ReadOnly = true;
        }

        private void ConfigurarPropiedadesDataGridView(DataGridView dgv)
        {
            // Evitar que el usuario pueda agregar filas
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            // Evitar que el usuario redimensione filas y columnas
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;

            // Deshabilitar ordenamiento de columnas
            dgv.AllowUserToOrderColumns = false;

            // Establecer modo de selección a celda individual
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;

            // Ajustar automáticamente el tamaño de las celdas
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Mostrar encabezados de fila para poder etiquetar las operaciones
            dgv.RowHeadersVisible = true;
            dgv.RowHeadersWidth = 80;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Hide();
        }

        private void button_Filtrar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar que se hayan ingresado números en los TextBox
                if (string.IsNullOrEmpty(textBox_Filas.Text) || string.IsNullOrEmpty(textBox_Columnas.Text))
                {
                    MessageBox.Show("Por favor ingrese valores para filas y columnas.", "Error", MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    return;
                }

                // Convertir los valores de los TextBox a enteros
                int filas = Convert.ToInt32(textBox_Filas.Text);
                int columnas = Convert.ToInt32(textBox_Columnas.Text);

                // Verificar que los valores sean positivos
                if (filas <= 0 || columnas <= 0)
                {
                    MessageBox.Show("Las filas y columnas deben ser números positivos.", "Error", MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    return;
                }

                // Configurar las matrices en los DataGridView
                ConfigurarDataGridView(dataGridView_M1, filas, columnas);
                ConfigurarDataGridView(dataGridView_M2, filas, columnas);

                // Configurar el DataGridView para resultados - con 4 filas para todas las operaciones
                ConfigurarDataGridViewResultados(dataGridView_M3, filas, columnas);

                // Limpiar la matriz traspuesta
                dataGridView_Traspuesta.Rows.Clear();
                dataGridView_Traspuesta.Columns.Clear();

                // Habilitar edición en M1 y M2
                dataGridView_M1.ReadOnly = false;
                dataGridView_M2.ReadOnly = false;

                // Indicar al usuario que puede proceder a llenar las matrices
                MessageBox.Show($"Se han creado las matrices de {filas}x{columnas}. Por favor, complete todos los campos.",
                    "Matrices creadas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor ingrese solo números enteros.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView(DataGridView dgv, int filas, int columnas)
        {
            // Limpiar el DataGridView
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            // Agregar columnas
            for (int i = 0; i < columnas; i++)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = $"col{i}";
                col.HeaderText = $"C{i}";
                dgv.Columns.Add(col);
            }

            // Agregar filas
            for (int i = 0; i < filas; i++)
            {
                dgv.Rows.Add();
                dgv.Rows[i].HeaderCell.Value = $"F{i}";
            }

            // Asegurar que las propiedades estén correctamente configuradas
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;

            // Opcional: Establecer un ancho mínimo para las columnas
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.MinimumWidth = 50;
            }
        }

        private void ConfigurarDataGridViewResultados(DataGridView dgv, int filas, int columnas)
        {
            // Limpiar el DataGridView
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            // Agregar columnas
            for (int i = 0; i < columnas; i++)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = $"col{i}";
                col.HeaderText = $"C{i}";
                dgv.Columns.Add(col);
            }

            // Agregar 4 filas (suma, resta, multiplicación, división) por cada fila original
            int totalFilas = filas * 4;
            for (int i = 0; i < totalFilas; i++)
            {
                dgv.Rows.Add();

                // Etiquetar las filas según la operación
                int operacionIndex = i / filas;
                int filaIndex = i % filas;

                string etiqueta = "";
                switch (operacionIndex)
                {
                    case 0:
                        etiqueta = $"SUMA F{filaIndex}";
                        dgv.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                        break;
                    case 1:
                        etiqueta = $"RESTA F{filaIndex}";
                        dgv.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        break;
                    case 2:
                        etiqueta = $"MULT F{filaIndex}";
                        dgv.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                        break;
                    case 3:
                        etiqueta = $"DIV F{filaIndex}";
                        dgv.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                        break;
                }

                dgv.Rows[i].HeaderCell.Value = etiqueta;
            }

            // Asegurar que sea de solo lectura
            dgv.ReadOnly = true;

            // Hacer que los encabezados de fila sean visibles
            dgv.RowHeadersVisible = true;
            dgv.RowHeadersWidth = 80;
        }

        private void button_Calcular_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar que ambas matrices tengan el mismo tamaño
                if (dataGridView_M1.Columns.Count == 0 || dataGridView_M2.Columns.Count == 0)
                {
                    MessageBox.Show("Primero debe crear las matrices usando el botón Filtrar.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verificar que los valores estén completos en ambas matrices
                if (!MatrizCompleta(dataGridView_M1) || !MatrizCompleta(dataGridView_M2))
                {
                    MessageBox.Show("Debe llenar todos los campos de ambas matrices.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Obtener dimensiones
                int filas = dataGridView_M1.Rows.Count;
                int columnas = dataGridView_M1.Columns.Count;

                // Crear matrices para operaciones
                double[,] matriz1 = ObtenerMatriz(dataGridView_M1);
                double[,] matriz2 = ObtenerMatriz(dataGridView_M2);

                // Calcular resultados de todas las operaciones
                double[,] resultadoSuma = RealizarOperacion(matriz1, matriz2, "suma");
                double[,] resultadoResta = RealizarOperacion(matriz1, matriz2, "resta");
                double[,] resultadoMultiplicacion = RealizarOperacion(matriz1, matriz2, "multiplicacion");

                // Intentar calcular la división (puede fallar si hay divisiones por cero)
                double[,] resultadoDivision;
                bool divisionExitosa = true;
                try
                {
                    resultadoDivision = RealizarOperacion(matriz1, matriz2, "division");
                }
                catch (DivideByZeroException)
                {
                    MessageBox.Show("No se pudo calcular la división porque hay elementos con valor 0 en la segunda matriz.",
                        "Error en división", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    resultadoDivision = new double[filas, columnas];
                    divisionExitosa = false;
                }

                // Mostrar resultados de todas las operaciones en el DataGridView_M3
                MostrarTodosLosResultados(resultadoSuma, resultadoResta, resultadoMultiplicacion, resultadoDivision, 
                    divisionExitosa, dataGridView_M3, filas, columnas);

                dataGridView_Traspuesta.Rows.Clear();
                dataGridView_Traspuesta.Columns.Clear();

                string mensaje = "Operaciones completadas:\n\n";
                mensaje += "✓ Suma\n";
                mensaje += "✓ Resta\n";
                mensaje += "✓ Multiplicación\n";

                if (divisionExitosa)
                    mensaje += "✓ División\n";
                else
                    mensaje += "✗ División (Error: división por cero)\n";

                mensaje += "\nTodos los resultados se muestran en la tabla de resultados.";
                MessageBox.Show(mensaje, "Cálculos completados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Todos los campos deben contener números válidos.", "Error de formato", MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool MatrizCompleta(DataGridView dgv)
        {
            foreach (DataGridViewRow fila in dgv.Rows)
            {
                foreach (DataGridViewCell celda in fila.Cells)
                {
                    if (celda.Value == null || string.IsNullOrWhiteSpace(celda.Value.ToString()))
                    {
                        return false;
                    }

                    if (!double.TryParse(celda.Value.ToString(), out _))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private double[,] ObtenerMatriz(DataGridView dgv)
        {
            int filas = dgv.Rows.Count;
            int columnas = dgv.Columns.Count;
            double[,] matriz = new double[filas, columnas];

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    matriz[i, j] = Convert.ToDouble(dgv.Rows[i].Cells[j].Value);
                }
            }

            return matriz;
        }

        private double[,] RealizarOperacion(double[,] matriz1, double[,] matriz2, string operacion)
        {
            int filas = matriz1.GetLength(0);
            int columnas = matriz1.GetLength(1);
            double[,] resultado = new double[filas, columnas];

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    switch (operacion.ToLower())
                    {
                        case "suma":
                            resultado[i, j] = matriz1[i, j] + matriz2[i, j];
                            break;
                        case "resta":
                            resultado[i, j] = matriz1[i, j] - matriz2[i, j];
                            break;
                        case "multiplicacion":
                            resultado[i, j] = matriz1[i, j] * matriz2[i, j];
                            break;
                        case "division":
                            // Verificar división por cero
                            if (matriz2[i, j] == 0)
                            {
                                throw new DivideByZeroException("No se puede dividir por cero.");
                            }
                            resultado[i, j] = matriz1[i, j] / matriz2[i, j];
                            break;
                        default:
                            throw new ArgumentException("Operación no válida.");
                    }
                }
            }

            return resultado;
        }

        private void MostrarTodosLosResultados(double[,] resultadoSuma, double[,] resultadoResta,
                                              double[,] resultadoMultiplicacion, double[,] resultadoDivision,
                                              bool divisionExitosa, DataGridView dgv, int filas, int columnas)
        {

            if (dgv.Rows.Count != filas * 4 || dgv.Columns.Count != columnas)
            {
                ConfigurarDataGridViewResultados(dgv, filas, columnas);
            }

            // Llenar los resultados de la suma
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    dgv.Rows[i].Cells[j].Value = resultadoSuma[i, j].ToString();
                }
            }

            // Llenar los resultados de la resta
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    dgv.Rows[i + filas].Cells[j].Value = resultadoResta[i, j].ToString();
                }
            }

            // Llenar los resultados de la multiplicación
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    dgv.Rows[i + (filas * 2)].Cells[j].Value = resultadoMultiplicacion[i, j].ToString();
                }
            }

            // Llenar los resultados de la división
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (divisionExitosa)
                    {
                        dgv.Rows[i + (filas * 3)].Cells[j].Value = resultadoDivision[i, j].ToString();
                    }
                    else
                    {
                        dgv.Rows[i + (filas * 3)].Cells[j].Value = "ERROR";
                    }
                }
            }
        }

        private void button_traspuesta_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView_M3.Columns.Count == 0 || dataGridView_M3.Rows.Count == 0)
                {
                    MessageBox.Show("Primero debe calcular las operaciones para generar la matriz M3.",
                        "Datos no disponibles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int filasOriginal = dataGridView_M3.Rows.Count;
                int columnasOriginal = dataGridView_M3.Columns.Count;
                dataGridView_Traspuesta.Rows.Clear();
                dataGridView_Traspuesta.Columns.Clear();

                for (int i = 0; i < filasOriginal; i++)
                {
                    DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                    col.Name = $"col{i}";
                    string headerText = dataGridView_M3.Rows[i].HeaderCell.Value?.ToString() ?? $"F{i}";
                    col.HeaderText = headerText;
                    dataGridView_Traspuesta.Columns.Add(col);
                }

                for (int i = 0; i < columnasOriginal; i++)
                {
                    dataGridView_Traspuesta.Rows.Add();
                    dataGridView_Traspuesta.Rows[i].HeaderCell.Value = $"C{i}";
                }

                for (int i = 0; i < filasOriginal; i++)
                {
                    for (int j = 0; j < columnasOriginal; j++)
                    {
                        if (dataGridView_M3.Rows[i].Cells[j].Value != null)
                        {
                            dataGridView_Traspuesta.Rows[j].Cells[i].Value = dataGridView_M3.Rows[i].Cells[j].Value.ToString();
                            dataGridView_Traspuesta.Rows[j].Cells[i].Style.BackColor =
                                dataGridView_M3.Rows[i].DefaultCellStyle.BackColor;
                        }
                        else
                        {
                            dataGridView_Traspuesta.Rows[j].Cells[i].Value = "";
                        }
                    }
                }

                dataGridView_Traspuesta.ReadOnly = true;
                dataGridView_Traspuesta.AllowUserToAddRows = false;
                dataGridView_Traspuesta.AllowUserToDeleteRows = false;
                dataGridView_Traspuesta.AllowUserToResizeRows = false;
                dataGridView_Traspuesta.AllowUserToResizeColumns = false;
                dataGridView_Traspuesta.AllowUserToOrderColumns = false;
                dataGridView_Traspuesta.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dataGridView_Traspuesta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView_Traspuesta.RowHeadersVisible = true;
                dataGridView_Traspuesta.RowHeadersWidth = 80;

                MessageBox.Show("Matriz transpuesta generada con éxito.", "Operación completada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error al generar la matriz transpuesta: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MManual_Load(object sender, EventArgs e)
        {

        }
    }
}