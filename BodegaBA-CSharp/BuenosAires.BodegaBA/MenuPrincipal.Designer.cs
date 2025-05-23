namespace BuenosAires.BodegaBA
{
    partial class ReservasANWO
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
            this.btnProductosEnBodega = new System.Windows.Forms.Button();
            this.btnGuiasDespacho = new System.Windows.Forms.Button();
            this.btnANWO = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Buenos Aires";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(121, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(484, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sistema de Bodega - Menú Principal";
            // 
            // btnProductosEnBodega
            // 
            this.btnProductosEnBodega.Location = new System.Drawing.Point(255, 116);
            this.btnProductosEnBodega.Name = "btnProductosEnBodega";
            this.btnProductosEnBodega.Size = new System.Drawing.Size(213, 64);
            this.btnProductosEnBodega.TabIndex = 2;
            this.btnProductosEnBodega.Text = "Consultar Productos en Bodega";
            this.btnProductosEnBodega.UseVisualStyleBackColor = true;
            this.btnProductosEnBodega.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGuiasDespacho
            // 
            this.btnGuiasDespacho.Location = new System.Drawing.Point(255, 208);
            this.btnGuiasDespacho.Name = "btnGuiasDespacho";
            this.btnGuiasDespacho.Size = new System.Drawing.Size(213, 64);
            this.btnGuiasDespacho.TabIndex = 3;
            this.btnGuiasDespacho.Text = "Administrar Guías de Despacho";
            this.btnGuiasDespacho.UseVisualStyleBackColor = true;
            // 
            // btnANWO
            // 
            this.btnANWO.Location = new System.Drawing.Point(255, 295);
            this.btnANWO.Name = "btnANWO";
            this.btnANWO.Size = new System.Drawing.Size(213, 64);
            this.btnANWO.TabIndex = 4;
            this.btnANWO.Text = "Reservar Equipos de ANWO";
            this.btnANWO.UseVisualStyleBackColor = true;
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(255, 378);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(213, 64);
            this.btnSalir.TabIndex = 5;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            // 
            // ReservasANWO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 471);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnANWO);
            this.Controls.Add(this.btnGuiasDespacho);
            this.Controls.Add(this.btnProductosEnBodega);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ReservasANWO";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnProductosEnBodega;
        private System.Windows.Forms.Button btnGuiasDespacho;
        private System.Windows.Forms.Button btnANWO;
        private System.Windows.Forms.Button btnSalir;
    }
}