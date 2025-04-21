using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ape_Issac
{
    public partial class MRandomica : Form
    {
        private Random random = new Random();

        // Colores simplificados
        private readonly Color colorPrimario = Color.FromArgb(100, 149, 237);
        private readonly Color colorSecundario = Color.FromArgb(252, 252, 252);

        // Colores para operaciones simplificados
        private readonly Color colorSuma = Color.LightBlue;
        private readonly Color colorResta = Color.LightGreen;
        private readonly Color colorMultiplicacion = Color.LightYellow;
        private readonly Color colorDivision = Color.LightPink;

        public MRandomica()
        {
            InitializeComponent();
        }

        private void MRandomica_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridViews();
            GenerarMatricesAleatorias();
            RealizarOperaciones();
            GenerarMatrizTraspuesta();
        }

        private void button_Generar_Click(object sender, EventArgs e)
        {
            GenerarMatricesAleatorias();
            RealizarOperaciones();
            GenerarMatrizTraspuesta();
        }

        private void ConfigurarDataGridViews()
        {
            foreach (var dgv in new[] { dataGridView_M1, dataGridView_M2, dataGridView_M3, dataGridView_Traspuesta })
            {
                // Configuraciones comunes básicas
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.RowHeadersVisible = true;
                dgv.BorderStyle = BorderStyle.None;
                dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                dgv.DefaultCellStyle.BackColor = colorSecundario;
                dgv.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dataGridView_M1.ReadOnly = true;
            dataGridView_M2.ReadOnly = true;
            dataGridView_M3.ReadOnly = true;
            dataGridView_Traspuesta.ReadOnly = true;
        }

        private void GenerarMatricesAleatorias()
        {
            int filas = random.Next(2, 6);
            int columnas = random.Next(2, 6);

            dataGridView_M1.RowCount = filas;
            dataGridView_M1.ColumnCount = columnas;
            dataGridView_M2.RowCount = filas;
            dataGridView_M2.ColumnCount = columnas;

            for (int j = 0; j < columnas; j++)
            {
                string columnName = $"C{j}";
                dataGridView_M1.Columns[j].HeaderText = columnName;
                dataGridView_M2.Columns[j].HeaderText = columnName;
            }

            for (int i = 0; i < filas; i++)
            {
                dataGridView_M1.Rows[i].HeaderCell.Value = $"F{i}";
                dataGridView_M2.Rows[i].HeaderCell.Value = $"F{i}";
            }

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    int valorM1 = random.Next(1, 10);
                    int valorM2 = random.Next(1, 10);

                    dataGridView_M1[j, i].Value = valorM1;
                    dataGridView_M2[j, i].Value = valorM2;
                }
            }
        }

        private void RealizarOperaciones()
        {
            int filas = dataGridView_M1.RowCount;
            int columnas = dataGridView_M1.ColumnCount;

            // Espacios entre cada operación
            int espacioExtraFilas = 1;

            // Calculamos el total de filas necesarias
            int totalFilas = filas * 4 + espacioExtraFilas * 4;

            dataGridView_M3.RowCount = totalFilas;
            dataGridView_M3.ColumnCount = columnas;

            // Establecer nombres de columnas
            for (int j = 0; j < columnas; j++)
            {
                dataGridView_M3.Columns[j].HeaderText = $"C{j}";
            }

            // Limpiar el dataGridView_M3
            for (int i = 0; i < totalFilas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    dataGridView_M3[j, i].Value = null;
                    dataGridView_M3[j, i].Style.BackColor = colorSecundario;
                    dataGridView_M3[j, i].Style.ForeColor = Color.Black;
                }
                dataGridView_M3.Rows[i].HeaderCell.Value = "";
            }

            // Índices para cada operación
            int inicioSuma = 0;
            int inicioResta = filas + espacioExtraFilas;
            int inicioMultiplicacion = 2 * filas + 2 * espacioExtraFilas;
            int inicioDivision = 3 * filas + 3 * espacioExtraFilas;

            // Configurar etiquetas
            ConfigurarEtiqueta(inicioSuma, "SUMA", colorSuma);
            ConfigurarEtiqueta(inicioResta, "RESTA", colorResta);
            ConfigurarEtiqueta(inicioMultiplicacion, "MULT", colorMultiplicacion);
            ConfigurarEtiqueta(inicioDivision, "DIV", colorDivision);

            // Realizar las operaciones
            for (int i = 0; i < filas; i++)
            {
                // Configurar headers de filas simplemente como F0, F1, etc.
                dataGridView_M3.Rows[inicioSuma + i + 1].HeaderCell.Value = $"F{i}";
                dataGridView_M3.Rows[inicioResta + i + 1].HeaderCell.Value = $"F{i}";
                dataGridView_M3.Rows[inicioMultiplicacion + i + 1].HeaderCell.Value = $"F{i}";
                dataGridView_M3.Rows[inicioDivision + i + 1].HeaderCell.Value = $"F{i}";

                for (int j = 0; j < columnas; j++)
                {
                    double valorM1 = Convert.ToDouble(dataGridView_M1[j, i].Value);
                    double valorM2 = Convert.ToDouble(dataGridView_M2[j, i].Value);

                    // Suma
                    var resultado = valorM1 + valorM2;
                    dataGridView_M3[j, inicioSuma + i + 1].Value = resultado;
                    dataGridView_M3[j, inicioSuma + i + 1].Style.BackColor = colorSuma;

                    // Resta
                    resultado = valorM1 - valorM2;
                    dataGridView_M3[j, inicioResta + i + 1].Value = resultado;
                    dataGridView_M3[j, inicioResta + i + 1].Style.BackColor = colorResta;

                    // Multiplicación
                    resultado = valorM1 * valorM2;
                    dataGridView_M3[j, inicioMultiplicacion + i + 1].Value = resultado;
                    dataGridView_M3[j, inicioMultiplicacion + i + 1].Style.BackColor = colorMultiplicacion;

                    // División
                    if (valorM2 != 0)
                    {
                        resultado = Math.Round(valorM1 / valorM2, 2);
                        dataGridView_M3[j, inicioDivision + i + 1].Value = resultado;
                        dataGridView_M3[j, inicioDivision + i + 1].Style.BackColor = colorDivision;
                    }
                    else
                    {
                        dataGridView_M3[j, inicioDivision + i + 1].Value = "ERROR";
                        dataGridView_M3[j, inicioDivision + i + 1].Style.BackColor = colorDivision;
                    }
                }
            }

            // Añadir separadores simples entre las secciones
            for (int j = 0; j < columnas; j++)
            {
                // Separador después de SUMA
                if (inicioResta - 1 >= 0)
                    dataGridView_M3[j, inicioResta - 1].Style.BackColor = Color.White;

                // Separador después de RESTA
                if (inicioMultiplicacion - 1 >= 0)
                    dataGridView_M3[j, inicioMultiplicacion - 1].Style.BackColor = Color.White;

                // Separador después de MULTIPLICACIÓN
                if (inicioDivision - 1 >= 0)
                    dataGridView_M3[j, inicioDivision - 1].Style.BackColor = Color.White;
            }
        }

        private void ConfigurarEtiqueta(int filaInicio, string etiqueta, Color colorBase)
        {
            // Configurar estilo del header de la fila
            dataGridView_M3.Rows[filaInicio].HeaderCell.Value = etiqueta;
            dataGridView_M3.Rows[filaInicio].HeaderCell.Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            for (int j = 0; j < dataGridView_M3.ColumnCount; j++)
            {
                dataGridView_M3[j, filaInicio].Style.BackColor = colorBase;
                dataGridView_M3[j, filaInicio].Value = etiqueta;
                dataGridView_M3[j, filaInicio].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }

            // Unir visualmente las celdas de la fila de título 
            if (dataGridView_M3.ColumnCount > 1)
            {
                dataGridView_M3[0, filaInicio].Value = etiqueta;
                for (int j = 1; j < dataGridView_M3.ColumnCount; j++)
                {
                    dataGridView_M3[j, filaInicio].Value = "";
                }
            }
        }

        private void GenerarMatrizTraspuesta()
        {
            int filas = dataGridView_M3.RowCount;
            int columnas = dataGridView_M3.ColumnCount;

            // En la matriz traspuesta, las filas y columnas se invierten
            dataGridView_Traspuesta.ColumnCount = filas;
            dataGridView_Traspuesta.RowCount = columnas;

            // Configurar nombres de filas en la matriz traspuesta (antes eran columnas)
            for (int i = 0; i < columnas; i++)
            {
                dataGridView_Traspuesta.Rows[i].HeaderCell.Value = $"F{i}";
            }

            // Configurar nombres de columnas para la matriz traspuesta (antes eran filas)
            for (int j = 0; j < filas; j++)
            {
                dataGridView_Traspuesta.Columns[j].HeaderText = $"C{j}";
            }

            // Transponer la matriz
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (dataGridView_M3[j, i].Value != null)
                    {
                        dataGridView_Traspuesta[i, j].Value = dataGridView_M3[j, i].Value;
                        dataGridView_Traspuesta[i, j].Style.BackColor = dataGridView_M3[j, i].Style.BackColor;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Hide();
        }
    }
}