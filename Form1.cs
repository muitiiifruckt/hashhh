using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace hashhh
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int r = 25;// len of block 
        int c = 20; // len of expander block
        int n = 3, m = 3, l = 5; // size of matrix

        private void button1_Click(object sender, EventArgs e)
        {
            hash();
        }
        private string int_to_binar_8bit(int a)
        {
            string binar = "";
            int[] b = new int[8]; //           // n - остаток после % деления из которого формируется число в 
            int n = 0;   //    двоичной системе исчисления
            int i = 0;                //будет выведено с конца для правильного отображения 

            while (a >= 1)
            {
                n = a % 2;
                b[i] = n;
                i++;
                a = a / 2;
            };
            for (i = (b.Length - 1); i >= 0; i--)
            {
                binar += b[i];
            }
            return binar;
        }

        private void function_f_xor_lines(ref int[,,] A)
        {
            for (int k = 1; k < l; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {

                        A[i, j, k] = (A[i, j, k] + A[i, j, k - 1]) % 2; // xor

                    }
                }
            }
        }
        private string XOR_16_bit(char aa , char bb)
        {
            string a = char.ToString(aa);
            string b = char.ToString(bb);

            int integer_a = Convert.ToInt32(a, 16);
            int integer_b = Convert.ToInt32(b, 16);
            int integer_c = (integer_a + integer_b) % 16;
            string c = Convert.ToString(integer_c, 16); 
            return c;
        }
        private void function_f_xor_columns(ref int[,,] A)
        {
            for (int k = 1; k < l; k++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < m; i++)
                    {

                        A[i, j, k] = (A[i, j, k] + A[i, j, k - 1]) % 2; // xor

                    }
                }
            }
        }
        private void function_f_transposition(ref int[,,] A)
        {
            for (int k = 0; k < l; k++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < m; i++)
                    {
                        // swap between A[i, j, k] and A[j, i, k]
                        int help = A[i, j, k];
                        A[i, j, k] = A[j, i, k];
                        A[j, i, k] = help;
                    }
                }
            }
        }
        private string array_sweep_from_colums(int[,,] A)
        {
            string text = "";
            for (int k = 0; k < l; k++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < m; i++)
                    {
                        text += A[i, j, k];
                    }
                }
            }
            return text;
        }
        private string get_16th_form(string sweep_array)
        {
            //Let's represent our hash in 16th form. For this,
            //let's break the number from the end into blocks of 4 bits,
            //and turn each block into the 16th digit
            int n = 45 / 4; // count of blocks
            int m = 4; // len of blocks
            string the16_form = "";
            for (int i = n - 1; i >= 0; i--) // from end
            {
                string text = "";
                for (int j = 0; j < m; j++)
                {
                    text += sweep_array[i * 4 + j];
                }
                int integer = Convert.ToInt32(text, 2);
                the16_form += Convert.ToString(integer,16);
            }
            the16_form += sweep_array[0]; // last element
            // reverse
            string help = "";
            for (int i = the16_form.Length - 1; i >= 0; i--)
            {
                help += the16_form[i];
            }
            the16_form = help;
            //
            return the16_form;
        }
        private void hash()
        {
            // step 1: get the binar cod
            string text = richTextBox_text.Text;
            string binar_text = "";
            byte[] bytes = Encoding.GetEncoding(1251).GetBytes(text);
            for (int i = 0; i < bytes.Length; i++)
            {
                binar_text += int_to_binar_8bit(bytes[i]);
            } // get the binar cod of message
            richTextBox_hash.Text = binar_text;

            // step  2: expand binar cod  "1" while until it becomes multiple r
            while (binar_text.Length % r != 0)
            {
                binar_text += 1;
            }

            string hash = "000000000000"; // will be hash after end algo
            int quantity_of_blocks = binar_text.Length / r;
            for (int block = 0; block < quantity_of_blocks; block++) // run this algorithm for every block
            {
                // step 3:  work from fisrt 25 symbols 
                string b1 = "";
                for (int i = 0; i < r; i++)
                {
                    b1 += binar_text[block * r + i];
                }
                // supplement b1  10 strind "01"
                string addition = "01";

                for (int i = 0; i < 10; i++)
                {
                    b1 += addition;
                }
                // step 4: create from b1 matrix 3*3*5
                int counter = 0;
                int[,,] A = new int[3, 3, 5];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        for (int k = 0; k < l; k++)
                        {
                            if (b1[counter]=='1')
                                A[i, j, k] = 1;
                            else
                                A[i, j, k] = 0;
                            counter++;
                        }
                    }
                }
                // step 5 : XOR 

                function_f_xor_lines(ref A);
                function_f_xor_columns(ref A);

                // step 6: transposition
                function_f_transposition(ref A);

                // step 7: repeat the fucntion_f 3 times
                int rounds = 3;
                for (int i = 0; i < rounds; i++)
                {
                    function_f_xor_lines(ref A);
                    function_f_xor_columns(ref A);
                    function_f_transposition(ref A);
                }
                // step 8 : it is necessary to “squeeze” / array sweep from colums
                string sweep_array = array_sweep_from_colums(A);
                string the16_form_of_sweep_array = get_16th_form(sweep_array);
                // step 9: XOR of hash of every block from modul 16
                string help = "";
                for (int i = 0; i < the16_form_of_sweep_array.Length; i++)
                {
                    help += XOR_16_bit(hash[i], the16_form_of_sweep_array[i]);
                }
                hash = help;
            }
            richTextBox_hash.Text = hash;
        }
    }
}
        
