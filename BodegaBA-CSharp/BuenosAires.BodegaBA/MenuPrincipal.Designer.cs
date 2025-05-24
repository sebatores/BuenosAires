namespace BuenosAires.BodegaBA
{
    partial class MenuPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConsultarProductos = new System.Windows.Forms.Button();
            this.btnAdministrarDespacho = new System.Windows.Forms.Button();
            this.btnANWO = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Buenos Aires";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(137, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(484, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sistema de Bodega - Menú Principal";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnConsultarProductos
            // 
            this.btnConsultarProductos.Location = new System.Drawing.Point(259, 103);
            this.btnConsultarProductos.Name = "btnConsultarProductos";
            this.btnConsultarProductos.Size = new System.Drawing.Size(252, 63);
            this.btnConsultarProductos.TabIndex = 2;
            this.btnConsultarProductos.Text = "Consultar Productos en Bodega";
            this.btnConsultarProductos.UseVisualStyleBackColor = true;
            // 
            // btnAdministrarDespacho
            // 
            this.btnAdministrarDespacho.Location = new System.Drawing.Point(259, 181);
            this.btnAdministrarDespacho.Name = "btnAdministrarDespacho";
            this.btnAdministrarDespacho.Size = new System.Drawing.Size(252, 63);
            this.btnAdministrarDespacho.TabIndex = 3;
            this.btnAdministrarDespacho.Text = "Administrar Guías de Despacho";
            this.btnAdministrarDespacho.UseVisualStyleBackColor = true;
            // 
            // btnANWO
            // 
            this.btnANWO.Location = new System.Drawing.Point(259, 272);
            this.btnANWO.Name = "btnANWO";
            this.btnANWO.Size = new System.Drawing.Size(252, 63);
            this.btnANWO.TabIndex = 4;
            this.btnANWO.Text = "Reservas Equipos de ANWO";
            this.btnANWO.UseVisualStyleBackColor = true;
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(259, 363);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(252, 63);
            this.btnSalir.TabIndex = 5;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            // 
            // MenuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnANWO);
            this.Controls.Add(this.btnAdministrarDespacho);
            this.Controls.Add(this.btnConsultarProductos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MenuPrincipal";
            this.Text = "MenuPrincipal";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConsultarProductos;
        private System.Windows.Forms.Button btnAdministrarDespacho;
        private System.Windows.Forms.Button btnANWO;
        private System.Windows.Forms.Button btnSalir;
    }
}