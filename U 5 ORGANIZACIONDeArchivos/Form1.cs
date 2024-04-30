using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace U_5_ORGANIZACIONDeArchivos
{
    public partial class Form1 : Form
    {
        private DataGridView dataGridViewSequential;
        private DataGridView dataGridViewIndexed;
        private DataGridView dataGridViewDirect;
        private TextBox textBoxInput;
        private Button buttonAdd;
        private Button buttonSearch;
        private Button buttonDelete;

        // Estructuras de datos para cada tipo de organización
        private List<string> sequentialList;
        private Dictionary<string, string> indexedDictionary;
        private Dictionary<int, string> directDictionary;


        private string placeholder = "Ingresa datos...";
        private bool isPlaceholder = true;

        public Form1()
        {
            InitializeComponents();

        }
        private void InitializeComponents()
        {
            // Inicializar las listas y diccionarios
            sequentialList = new List<string>();
            indexedDictionary = new Dictionary<string, string>();
            directDictionary = new Dictionary<int, string>();

            textBoxInput = new TextBox
            {
                Dock = DockStyle.Top,
                Text = placeholder // Inicializa el TextBox con el marcador de posición
            };

            // Asigna eventos al TextBox para manejar el marcador de posición
            textBoxInput.Enter += TextBoxInput_Enter;
            textBoxInput.Leave += TextBoxInput_Leave;




            // Configurar controles de entrada
           // textBoxInput = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Ingresa datos..." };
            buttonAdd = new Button { Text = "Agregar", Dock = DockStyle.Top };
            buttonSearch = new Button { Text = "Buscar", Dock = DockStyle.Top };
            buttonDelete = new Button { Text = "Eliminar", Dock = DockStyle.Top };

            // Configurar DataGridViews
            dataGridViewSequential = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Name = "Secuencial"
            };

            dataGridViewIndexed = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Name = "Indexado"
            };

            dataGridViewDirect = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Name = "Directo"
            };
            dataGridViewSequential.Columns.Add("Data", "Datos Secuenciales");
            dataGridViewIndexed.Columns.Add("Data", "Datos Indexados");
            dataGridViewDirect.Columns.Add("Key", "Clave");
            dataGridViewDirect.Columns.Add("Data", "Datos Directos");

            // Configurar los eventos de los botones
            buttonAdd.Click += ButtonAdd_Click;
            buttonSearch.Click += ButtonSearch_Click;
            buttonDelete.Click += ButtonDelete_Click;

            // Crear panel de control y agregar botones y cuadro de texto
            var controlPanel = new Panel { Dock = DockStyle.Top, Height = 100 };
            controlPanel.Controls.Add(buttonDelete);
            controlPanel.Controls.Add(buttonSearch);
            controlPanel.Controls.Add(buttonAdd);
            controlPanel.Controls.Add(textBoxInput);

            // Crear panel de DataGridViews
            var dataPanel = new Panel { Dock = DockStyle.Fill };
            var sequentialGroup = new GroupBox { Text = "Organización Secuencial", Dock = DockStyle.Top, Height = 150 };
            var indexedGroup = new GroupBox { Text = "Organización Indexada", Dock = DockStyle.Top, Height = 150 };
            var directGroup = new GroupBox { Text = "Organización Directa", Dock = DockStyle.Top, Height = 150 };

            // Añadir DataGridViews a sus grupos
            sequentialGroup.Controls.Add(dataGridViewSequential);
            indexedGroup.Controls.Add(dataGridViewIndexed);
            directGroup.Controls.Add(dataGridViewDirect);

            // Agregar grupos al panel de datos
            dataPanel.Controls.Add(sequentialGroup);
            dataPanel.Controls.Add(indexedGroup);
            dataPanel.Controls.Add(directGroup);

            // Agregar paneles al formulario
            Controls.Add(dataPanel);
            Controls.Add(controlPanel);

            // Configurar el tamaño del formulario
            this.Width = 1000;
            this.Height = 500;
            this.Text = "Demostración de Organización de Archivos";
        }
        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            // Buscar datos en cada tipo de organización
            string searchKey = textBoxInput.Text.Trim();
            if (string.IsNullOrEmpty(searchKey)) return;

            // Búsqueda secuencial
            string sequentialResult = sequentialList.Find(data => data == searchKey);
            if (sequentialResult != null)
            {
                MessageBox.Show($"Secuencial: Encontrado - {sequentialResult}");
            }

            // Búsqueda indexada
            if (indexedDictionary.ContainsKey(searchKey))
            {
                MessageBox.Show($"Indexado: Encontrado - {searchKey}");
            }

            // Búsqueda directa
            if (int.TryParse(searchKey, out int directKey) && directDictionary.ContainsKey(directKey))
            {
                MessageBox.Show($"Directo: Encontrado - {directDictionary[directKey]}");
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            // Eliminar datos de cada tipo de organización
            string deleteKey = textBoxInput.Text.Trim();
            if (string.IsNullOrEmpty(deleteKey)) return;

            // Eliminar de la organización secuencial
            sequentialList.Remove(deleteKey);
            RefreshDataGridView(dataGridViewSequential, sequentialList);

            // Eliminar de la organización indexada
            indexedDictionary.Remove(deleteKey);
            RefreshDataGridView(dataGridViewIndexed, indexedDictionary.Values);

            // Eliminar de la organización directa
            if (int.TryParse(deleteKey, out int directKey) && directDictionary.ContainsKey(directKey))
            {
                directDictionary.Remove(directKey);
                RefreshDataGridView(dataGridViewDirect, directDictionary);
            }

            textBoxInput.Text = string.Empty;
        }

        private void RefreshDataGridView<T>(DataGridView dataGridView, IEnumerable<T> data)
        {
            // Refrescar DataGridView con los datos proporcionados
            dataGridView.Rows.Clear();
            foreach (var item in data)
            {
                dataGridView.Rows.Add(item);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            // Agregar datos a cada tipo de organización
            string data = textBoxInput.Text.Trim();
            if (string.IsNullOrEmpty(data)) return;

            // Organización secuencial
            sequentialList.Add(data);
            dataGridViewSequential.Rows.Add(data);

            // Organización indexada
            indexedDictionary[data] = data;
            dataGridViewIndexed.Rows.Add(data);

            // Organización directa
            int key = directDictionary.Count;
            directDictionary[key] = data;
            dataGridViewDirect.Rows.Add(key, data);

            // Limpia el cuadro de texto
            textBoxInput.Text = string.Empty;
        }
        private void TextBoxInput_Enter(object sender, EventArgs e)
        {
            if (isPlaceholder)
            {
                // Borrar el marcador de posición
                textBoxInput.Text = string.Empty;
                isPlaceholder = false;
                textBoxInput.ForeColor = System.Drawing.Color.Black; // Cambia el color del texto a negro
            }
        }
        private void TextBoxInput_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxInput.Text))
            {
                // Restablecer el marcador de posición
                textBoxInput.Text = placeholder;
                isPlaceholder = true;
                textBoxInput.ForeColor = System.Drawing.Color.Gray; // Cambia el color del texto a gris
            }
        }

    }
}

