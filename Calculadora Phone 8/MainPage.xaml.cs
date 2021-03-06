﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=391641

namespace Calculadora_Phone_8
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Initialize();
        }

        private void Initialize()
        {
            Border Cbutton = (Border)DeleteButtons.Children[0];
            Border CEbutton = (Border)DeleteButtons.Children[1];
            Cbutton.PointerPressed += button_PointerPressed;
            Cbutton.PointerReleased += button_PointerReleased;
            Cbutton.PointerCanceled += button_PointerReleased;
            Cbutton.PointerExited += button_PointerReleased;

            CEbutton.PointerPressed += button_PointerPressed;
            CEbutton.PointerReleased += button_PointerReleased;
            CEbutton.PointerCanceled += button_PointerReleased;
            CEbutton.PointerExited += button_PointerReleased;

            foreach (var item in NandOButtons.Children)
            {
                if (item is Border)
                {
                    Border bd = (Border)item;
                    bd.PointerPressed += button_PointerPressed;
                    bd.PointerReleased += button_PointerReleased;
                    bd.PointerCanceled += button_PointerReleased;
                    bd.PointerExited += button_PointerReleased;
                }
            }
        }

        void button_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Border bd = (Border)sender;
            bd.Background = ColorReleased;
        }

        void button_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Border bd = (Border)sender;
            bd.Background = ColorPressed;
        }

        /// <summary>
        /// Se invoca cuando esta página se va a mostrar en un objeto Frame.
        /// </summary>
        /// <param name="e">Datos de evento que describen cómo se llegó a esta página.
        /// Este parámetro se usa normalmente para configurar la página.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Preparar la página que se va a mostrar aquí.

            // TODO: Si la aplicación contiene varias páginas, asegúrese de
            // controlar el botón para retroceder del hardware registrándose en el
            // evento Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si usa NavigationHelper, que se proporciona en algunas plantillas,
            // el evento se controla automáticamente.
        }

        private void Border_NumberPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Border bd = sender as Border;
            String tx = ((TextBlock)bd.Child).Text;
            if (tx.Equals("."))
            {
                if (isNumber(last))
                {
                    if (!output.Text.Contains("."))
                    {
                        output.Text += tx;
                        last = tx;
                    }
                }
                else if (isOperation(last))
                {
                    output.Text = "0.";
                    last = tx;
                }
                else
                {
                    if (!last.Equals("."))
                    {
                        output.Text = "0.";
                        last = tx;
                    }
                }
            } 
            else 
            {
                if (isNumber(last))
                {
                    if (!output.Text.Equals("0"))
                    {
                        output.Text += tx;
                        
                    }
                    else
                    {
                        output.Text = tx;
                    }
                    last = tx;
                }
                else if (isOperation(last))
                {
                    output.Text = tx;
                    last = tx;
                }
                else
                {
                    if (!last.Equals("."))
                    {
                        output.Text = tx;
                        last = tx;
                    }
                    else
                    {
                        output.Text += tx;
                        last = tx;
                    }
                }
            }
        }

        private void Border_OperationPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Border bd = sender as Border;
            String tx = ((TextBlock)bd.Child).Text;
            if (isOperation(last))
            {
                operador = tx;
                last = tx;
            }
            else if (isNumber(last))
            {
                if (operador.Equals("")) 
                {
                    operando1 = float.Parse(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                    operador = tx;
                    last = tx;
                }
                else
                {
                    operando2 = float.Parse(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                    operando1 = Execute();
                    operador = tx;
                    output.Text = operando1 + "";
                    last = tx;
                }
            } 
            else if (last.Equals("="))
            {
                operando1 = float.Parse(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                operador = tx;
                last = tx;
            }
        }

        private void Border_DeletePointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Border bd = sender as Border;
            String tx = ((TextBlock)bd.Child).Text;
            if (tx.Equals("C"))
            {
                output.Text = "0";
                last = "";
                operador = "";
            }
            else
            {
                output.Text = "0";
            }
        }

        private void Border_EqualPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Border bd = sender as Border;
            String tx = ((TextBlock)bd.Child).Text;
            if (isNumber(last) && operador!="")
            {
                if (float.Parse(output.Text, System.Globalization.CultureInfo.InvariantCulture) != 0.0f)
                {
                    operando2 = float.Parse(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                    output.Text = Execute() + "";
                    operador = "";
                    last = tx;
                }
            }
        }

        private bool isNumber(String tx)
        {
            try
            {
                if (tx.Equals("."))
                    return false;
                float.Parse(tx, System.Globalization.CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool isOperation(String op) 
        {
            try
            {
                if (op.Equals(".") || op.Equals(""))
                    return false;
                float.Parse(op, System.Globalization.CultureInfo.InvariantCulture);
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private float Execute()
        {
            if (operador.Equals("*"))
            {
                return operando1 * operando2;
            }
            else if (operador.Equals("/"))
            {
                return operando1 / operando2;
            }
            else if (operador.Equals("+"))
            {
                return operando1 + operando2;
            }
            else if (operador.Equals("-"))
            {
                return operando1 - operando2;
            }
            return 0.0f;
        }

        SolidColorBrush ColorPressed = new SolidColorBrush(Color.FromArgb(255, 81, 126, 172));
        SolidColorBrush ColorReleased = new SolidColorBrush(Color.FromArgb(255, 128, 162, 195));
        String last = "";
        float operando1 = 0.0f;
        float operando2 = 0.0f;
        String operador = "";
    }
}
