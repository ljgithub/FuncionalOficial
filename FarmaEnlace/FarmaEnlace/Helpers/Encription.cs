using System;
using System.Collections.Generic;
using System.Text;

namespace FarmaEnlace.Helpers
{
    public class Encription
    {
        public string tipo;
        public string codigo;
        public string identificacion;
        public int minutos;

        public int p;
        public int k;
        public int l;

        public Encription()
        {
        }

        public string GetCode(string type, string identification)
        {
            return Encriptador(Common.RemoveAccentsWithNormalization(type).ToLowerInvariant(), identification);
        }

        public string Encriptador(string tipo, string identificacion)
        {
            var codigo = "";
            var fecha = DateTime.Now;
            var minuto = fecha.Minute;
            var hora = fecha.Hour;

            var encripted = "";

            string horaStr;

            string minutoStr;

            var milisegundo = int.Parse(fecha.Millisecond.ToString().PadRight(3,'0')[1].ToString() + fecha.Millisecond.ToString().PadRight(3, '0')[2].ToString());
            var milisegundoStr = ConvertToBase(milisegundo, 36).ToUpperInvariant();
            string[] numerosPasaporte;

            while (milisegundoStr.Length < 2)
            {
                milisegundoStr = '0' + milisegundoStr;
            }

            if (hora < 10)
            {
                horaStr = '0' + hora.ToString();
            }
            else
            {
                horaStr = hora.ToString();
            }

            if (minuto < 10)
            {
                minutoStr = '0' + minuto.ToString();
            }
            else
            {
                minutoStr = minuto.ToString();
            }

            if (tipo.Equals("cedula", StringComparison.InvariantCultureIgnoreCase))
            {
                codigo += identificacion[2];
                codigo += identificacion[5];
                codigo += identificacion[9];
            }
            else if (tipo.Equals("ruc", StringComparison.InvariantCultureIgnoreCase))
            {
                codigo += identificacion[3];
                codigo += identificacion[7];
                codigo += identificacion[12];
            }
            else if (tipo.Equals("pasaporte", StringComparison.InvariantCultureIgnoreCase))
            {
                numerosPasaporte = dividirPasaporte(identificacion.ToUpperInvariant());
                codigo += numerosPasaporte[0];
                codigo += numerosPasaporte[2];
                codigo += numerosPasaporte[5];
            }
            else
            {
                return "Tipo de identificacion no valida!";
            }
            codigo += minutoStr;
            codigo = ConvertToBase(Convert.ToInt32(codigo), 36).ToString().ToUpperInvariant();

            while (codigo.Length < 4)
            {
                codigo = '0' + codigo;
            }

            codigo += milisegundoStr;
            encripted = run_permutacion(milisegundo, codigo);

            return encripted.ToLowerInvariant();
        }
        public String ConvertToBase(int num, int nbase)
        {
            String chars = "0123456789abcdefghijklmnopqrstuvwxyz";

            // check if we can convert to another base
            if (nbase < 2 || nbase > chars.Length)
                return "";

            int r;
            String newNumber = "";

            // in r we have the offset of the char that was converted to the new base
            while (num >= nbase)
            {
                r = num % nbase;
                newNumber = chars[r] + newNumber;
                num = num / nbase;
            }
            // the last number to convert
            newNumber = chars[num] + newNumber;

            return newNumber;
        }



        public static int DecodeBase36(string input)
        {
            string CharList = "0123456789abcdefghijklmnopqrstuvwxyz";
            input = input.ToLower();

            string reversed = "";

            for (int i = input.Length - 1; i >= 0; i--)
            {
                reversed = reversed + input[i];
            }

            long result = 0;
            int pos = 0;
            foreach (char c in reversed)
            {
                result += CharList.IndexOf(c) * (long)Math.Pow(36, pos);
                pos++;
            }
            return int.Parse(result.ToString());
        }

        /*Funcion que permuta un codigo de 6 digitos de acuerdo a una tabla de permutacion, num: numero de permutacion*/
        public string run_permutacion(int num, string codigo)
        {
            var permutacion = get_permutacion(num);
            int[] posiciones = new int[codigo.Length];
            for (var p = 0; p < permutacion.Length; p++)
            {
                posiciones[p] = Int32.Parse(permutacion[p] + "");
            }
            var codigo_permutado = "";
            for (var p = 0; p < permutacion.Length; p++)
            {
                codigo_permutado += codigo[(posiciones[p])];
            }
            return codigo_permutado;
        }


        /*Funciones que revierte una permutacion de un codigo de 6 o 7 digitos de acuerdo a una tabla de permutacion:
        num: numero de permutacion*/
        public string run_permutacion_invertido(int num, string codigo_permutado)
        {
            var permutacion = get_permutacion(num);
            int[] posiciones = new int[codigo_permutado.Length];
            for (var p = 0; p < permutacion.Length; p++)
            {
                posiciones[p] = Int32.Parse(permutacion[p] + "");
            }
            string[] codigo = new string[codigo_permutado.Length];
            string codigoStr = "";
            for (var p = 0; p < permutacion.Length; p++)
            {
                codigo[posiciones[p]] = codigo_permutado[p] + "";
            }
            for (var p = 0; p < permutacion.Length; p++)
            {
                codigoStr += codigo[p];
            }
            return codigoStr;
        }
        public string run_permutacion_invertido_7(int num, string codigo_permutado)
        {
            var permutacion = get_permutacion_7(num);
            int[] posiciones = new int[codigo_permutado.Length];
            for (var p = 0; p < permutacion.Length; p++)
            {
                posiciones[p] = Int32.Parse(permutacion[p] + "");
            }
            string[] codigo = new string[codigo_permutado.Length];
            string codigoStr = "";
            for (var p = 0; p < permutacion.Length; p++)
            {
                codigo[posiciones[p]] = codigo_permutado[p] + "";
            }
            for (var p = 0; p < permutacion.Length; p++)
            {
                codigoStr += codigo[p];
            }
            return codigoStr;
        }
        /*Funciones que devuelven una permutacion fijada de acuerdo a su posicion o numero de permutacion*/
        public string get_permutacion(int num)
        {
            var permutacion = "";
            switch (num)
            {
                case 0: permutacion = "103245"; break;
                case 1: permutacion = "123045"; break;
                case 2: permutacion = "301245"; break;
                case 3: permutacion = "321045"; break;
                case 4: permutacion = "203145"; break;
                case 5: permutacion = "213045"; break;
                case 6: permutacion = "302145"; break;
                case 7: permutacion = "312045"; break;
                case 8: permutacion = "021345"; break;
                case 9: permutacion = "031245"; break;
                case 10: permutacion = "120345"; break;
                case 11: permutacion = "130245"; break;
                case 12: permutacion = "012345"; break;
                case 13: permutacion = "032145"; break;
                case 14: permutacion = "210345"; break;
                case 15: permutacion = "230145"; break;
                case 16: permutacion = "013245"; break;
                case 17: permutacion = "023145"; break;
                case 18: permutacion = "102345"; break;
                case 19: permutacion = "132045"; break;
                case 20: permutacion = "201345"; break;
                case 21: permutacion = "231045"; break;
                case 22: permutacion = "310245"; break;
                case 23: permutacion = "320145"; break;
                case 24: permutacion = "103245"; break;
                case 25: permutacion = "123045"; break;
                case 26: permutacion = "301245"; break;
                case 27: permutacion = "321045"; break;
                case 28: permutacion = "203145"; break;
                case 29: permutacion = "213045"; break;
                case 30: permutacion = "302145"; break;
                case 31: permutacion = "312045"; break;
                case 32: permutacion = "021345"; break;
                case 33: permutacion = "031245"; break;
                case 34: permutacion = "120345"; break;
                case 35: permutacion = "130245"; break;
                case 36: permutacion = "012345"; break;
                case 37: permutacion = "032145"; break;
                case 38: permutacion = "210345"; break;
                case 39: permutacion = "230145"; break;
                case 40: permutacion = "013245"; break;
                case 41: permutacion = "023145"; break;
                case 42: permutacion = "102345"; break;
                case 43: permutacion = "132045"; break;
                case 44: permutacion = "201345"; break;
                case 45: permutacion = "231045"; break;
                case 46: permutacion = "310245"; break;
                case 47: permutacion = "320145"; break;
                case 48: permutacion = "103245"; break;
                case 49: permutacion = "123045"; break;
                case 50: permutacion = "301245"; break;
                case 51: permutacion = "321045"; break;
                case 52: permutacion = "203145"; break;
                case 53: permutacion = "213045"; break;
                case 54: permutacion = "302145"; break;
                case 55: permutacion = "312045"; break;
                case 56: permutacion = "021345"; break;
                case 57: permutacion = "031245"; break;
                case 58: permutacion = "120345"; break;
                case 59: permutacion = "130245"; break;
                case 60: permutacion = "012345"; break;
                case 61: permutacion = "032145"; break;
                case 62: permutacion = "210345"; break;
                case 63: permutacion = "230145"; break;
                case 64: permutacion = "013245"; break;
                case 65: permutacion = "023145"; break;
                case 66: permutacion = "102345"; break;
                case 67: permutacion = "132045"; break;
                case 68: permutacion = "201345"; break;
                case 69: permutacion = "231045"; break;
                case 70: permutacion = "310245"; break;
                case 71: permutacion = "320145"; break;
                case 72: permutacion = "103245"; break;
                case 73: permutacion = "123045"; break;
                case 74: permutacion = "301245"; break;
                case 75: permutacion = "321045"; break;
                case 76: permutacion = "203145"; break;
                case 77: permutacion = "213045"; break;
                case 78: permutacion = "302145"; break;
                case 79: permutacion = "312045"; break;
                case 80: permutacion = "021345"; break;
                case 81: permutacion = "031245"; break;
                case 82: permutacion = "120345"; break;
                case 83: permutacion = "130245"; break;
                case 84: permutacion = "012345"; break;
                case 85: permutacion = "032145"; break;
                case 86: permutacion = "210345"; break;
                case 87: permutacion = "230145"; break;
                case 88: permutacion = "013245"; break;
                case 89: permutacion = "023145"; break;
                case 90: permutacion = "102345"; break;
                case 91: permutacion = "132045"; break;
                case 92: permutacion = "201345"; break;
                case 93: permutacion = "231045"; break;
                case 94: permutacion = "310245"; break;
                case 95: permutacion = "320145"; break;
                case 96: permutacion = "103245"; break;
                case 97: permutacion = "123045"; break;
                case 98: permutacion = "301245"; break;
                case 99: permutacion = "321045"; break;
            }
            return permutacion;
        }
        public string get_permutacion_7(int num)
        {
            var permutacion = "";
            switch (num)
            {
                case 0: permutacion = "01342765"; break;
                case 1: permutacion = "01345267"; break;
                case 2: permutacion = "01345276"; break;
                case 3: permutacion = "01345627"; break;
                case 4: permutacion = "01345672"; break;
                case 5: permutacion = "01345726"; break;
                case 6: permutacion = "01345762"; break;
                case 7: permutacion = "01346257"; break;
                case 8: permutacion = "01346275"; break;
                case 9: permutacion = "01346527"; break;
                case 10: permutacion = "01346572"; break;
                case 11: permutacion = "01346725"; break;
                case 12: permutacion = "01346752"; break;
                case 13: permutacion = "01347256"; break;
                case 14: permutacion = "01347265"; break;
                case 15: permutacion = "01347526"; break;
                case 16: permutacion = "01347562"; break;
                case 17: permutacion = "01347625"; break;
                case 18: permutacion = "01347652"; break;
                case 19: permutacion = "01352467"; break;
                case 20: permutacion = "01352476"; break;
                case 21: permutacion = "01352647"; break;
                case 22: permutacion = "01352674"; break;
                case 23: permutacion = "01352746"; break;
                case 24: permutacion = "01352764"; break;
                case 25: permutacion = "01354267"; break;
                case 26: permutacion = "01354276"; break;
                case 27: permutacion = "01354627"; break;
                case 28: permutacion = "01354672"; break;
                case 29: permutacion = "01354726"; break;
                case 30: permutacion = "01354762"; break;
                case 31: permutacion = "01356247"; break;
                case 32: permutacion = "01356274"; break;
                case 33: permutacion = "01356427"; break;
                case 34: permutacion = "01356472"; break;
                case 35: permutacion = "01356724"; break;
                case 36: permutacion = "01356742"; break;
                case 37: permutacion = "01357246"; break;
                case 38: permutacion = "01357264"; break;
                case 39: permutacion = "01357426"; break;
                case 40: permutacion = "01357462"; break;
                case 41: permutacion = "01357624"; break;
                case 42: permutacion = "01357642"; break;
                case 43: permutacion = "01362457"; break;
                case 44: permutacion = "01362475"; break;
                case 45: permutacion = "01362547"; break;
                case 46: permutacion = "01362574"; break;
                case 47: permutacion = "01362745"; break;
                case 48: permutacion = "01362754"; break;
                case 49: permutacion = "01364257"; break;
                case 50: permutacion = "01364275"; break;
                case 51: permutacion = "01364527"; break;
                case 52: permutacion = "01364572"; break;
                case 53: permutacion = "01364725"; break;
                case 54: permutacion = "01364752"; break;
                case 55: permutacion = "01365247"; break;
                case 56: permutacion = "01365274"; break;
                case 57: permutacion = "01365427"; break;
                case 58: permutacion = "01365472"; break;
                case 59: permutacion = "01365724"; break;
                case 60: permutacion = "01365742"; break;
                case 61: permutacion = "01367245"; break;
                case 62: permutacion = "01367254"; break;
                case 63: permutacion = "01367425"; break;
                case 64: permutacion = "01367452"; break;
                case 65: permutacion = "01367524"; break;
                case 66: permutacion = "01367542"; break;
                case 67: permutacion = "01372456"; break;
                case 68: permutacion = "01372465"; break;
                case 69: permutacion = "01372546"; break;
                case 70: permutacion = "01372564"; break;
                case 71: permutacion = "01372645"; break;
                case 72: permutacion = "01372654"; break;
                case 73: permutacion = "01374256"; break;
                case 74: permutacion = "01374265"; break;
                case 75: permutacion = "01374526"; break;
                case 76: permutacion = "01374562"; break;
                case 77: permutacion = "01374625"; break;
                case 78: permutacion = "01374652"; break;
                case 79: permutacion = "01375246"; break;
                case 80: permutacion = "01375264"; break;
                case 81: permutacion = "01375426"; break;
                case 82: permutacion = "01375462"; break;
                case 83: permutacion = "01375624"; break;
                case 84: permutacion = "01375642"; break;
                case 85: permutacion = "01376245"; break;
                case 86: permutacion = "01376254"; break;
                case 87: permutacion = "01376425"; break;
                case 88: permutacion = "01376452"; break;
                case 89: permutacion = "01376524"; break;
                case 90: permutacion = "01376542"; break;
                case 91: permutacion = "01423567"; break;
                case 92: permutacion = "01423576"; break;
                case 93: permutacion = "01423657"; break;
                case 94: permutacion = "01423675"; break;
                case 95: permutacion = "01423756"; break;
                case 96: permutacion = "01423765"; break;
                case 97: permutacion = "01425367"; break;
                case 98: permutacion = "01425376"; break;
                case 99: permutacion = "01425637"; break;
                case 100: permutacion = "01425673"; break;
                case 101: permutacion = "01425736"; break;
                case 102: permutacion = "01425763"; break;
                case 103: permutacion = "01426357"; break;
                case 104: permutacion = "01426375"; break;
                case 105: permutacion = "01426537"; break;
                case 106: permutacion = "01426573"; break;
                case 107: permutacion = "01426735"; break;
                case 108: permutacion = "01426753"; break;
                case 109: permutacion = "01427356"; break;
                case 110: permutacion = "01427365"; break;
                case 111: permutacion = "01427536"; break;
                case 112: permutacion = "01427563"; break;
                case 113: permutacion = "01427635"; break;
                case 114: permutacion = "01427653"; break;
                case 115: permutacion = "01432567"; break;
                case 116: permutacion = "01432576"; break;
                case 117: permutacion = "01432657"; break;
                case 118: permutacion = "01432675"; break;
                case 119: permutacion = "01432756"; break;
                case 120: permutacion = "01432765"; break;
            }
            return permutacion;
        }
        /*Obtener el numero de la provincia de acuerdo al codigo*/
        public string provincia_num(string codigo)
        {
            var str = "abcdefghijklmnopqrstuvwx";
            var resultado = "";
            for (var i = 1; i <= str.Length; i++)
            {
                if (codigo[0].Equals(str[i - 1]))
                {
                    resultado = i + "";
                }
            }
            return pad(resultado, 2);
        }
        /*Funcion para dar formato a un numero de modo que siempre sea
        o tenga una longitud fija: 00001 para pad(1,5) por ejemplo*/
        public string pad(string num, int size)
        {
            var s = num + "";
            while (s.Length < size) s = "0" + s;
            return s;
        }
        /*Funcion que revierte un codigo de 2 digitos en un numero k*/
        public int get_k(string code)
        {
            var code_parts = code;
            return tablas_k_invertido(code_parts[0] + "", code_parts[1] + "");
        }
        /*Funcion que de acuerdo a dos digitos busca una tabla, determina
        la posicion del item en la tabla y determina el k normal(numero)*/
        public int tablas_k_invertido(string tabla_cod, string item)
        {
            var tabla = "";
            int tabla_indice = 0;
            switch (tabla_cod)
            {
                case "0": tabla_indice = 0; tabla = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; break;
                case "1": tabla_indice = 1; tabla = "APQRSTUVWXYZ012345BCDEFGHIJKLMNO6789abcdefghijklmnopqrstuvwxyz"; break;
                case "2": tabla_indice = 2; tabla = "abcdefghijklmnopqrstuv456789ABCDEFGHIwxyz0123JKLMNOPQRSTUVWXYZ"; break;
                case "3": tabla_indice = 3; tabla = "0123456789abjklmnopqrstuvwxyzABCDEFGHcdefghiIJKLMNOPQRSTUVWXYZ"; break;
                case "4": tabla_indice = 4; tabla = "012345yzABCDEFGHIJKLMNOPQRSTUVWXYZ6789abcdefghijklmnopqrstuvwx"; break;
                case "5": tabla_indice = 5; tabla = "0123456hijklmnopqrstuvwxyzABCDEFGHIJKLMNO789abcdefgPQRSTUVWXYZ"; break;
                case "6": tabla_indice = 6; tabla = "bcdefghijklmnopqrstuv012GHIJKLMN3456789awxyzABCDEFOPQRSTUVWXYZ"; break;
                case "7": tabla_indice = 7; tabla = "0tuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789abcdefghijklmnopqrs"; break;
                case "8": tabla_indice = 8; tabla = "012yzABCDEFGHIJKLMNbcdefghijklmnOP3456789aopqrstuvwxQRSTUVWXYZ"; break;
                case "9": tabla_indice = 9; tabla = "0123zAB6789abcdefghijklmnopqrstuvwxySTUVWXYCDEFGHIJKLMNOPQR45Z"; break;
                case "a": tabla_indice = 10; tabla = "01qrstuvwxyzABCDEFGHIJKLMNOPQRST23456789abcdefghijklmnopUVWXYZ"; break;
                case "b": tabla_indice = 11; tabla = "ABCDEF456789abcdefghijklmnopqrstGHIJKLMNOPQRSTUVWXYZ0123uvwxyz"; break;
                case "c": tabla_indice = 12; tabla = "abcdeqrstuv456789fDEFGHIwxyz0123JKLMNOPQRghijklmnopABCSTUVWXYZ"; break;
                case "d": tabla_indice = 13; tabla = "01234567ghiIJKLMNOPQRS89abjklmnopqrstuvwxyzABCDEFGHcdefTUVWXYZ"; break;
                case "e": tabla_indice = 14; tabla = "01234hijklmnopqrstuv5yzABCDEFGHIJKLMNOPQRSTUVWXYZ6789abcdefgwx"; break;
                case "f": tabla_indice = 15; tabla = "01IJKLMNO789abcdefgPQRST23456hijklmnopqrstuvwxyzABCDEFGHUVWXYZ"; break;
                case "g": tabla_indice = 16; tabla = "bc89awxyzABCDEFOPQRSTUVWdefghijklmnopqrstuv012GHIJKLMN34567XYZ"; break;
                case "h": tabla_indice = 17; tabla = "0tuvwxyz89abcABCDEFGHIJKLMNOPQRSTUVWXYZ1234567defghijklmnopqrs"; break;
                case "i": tabla_indice = 18; tabla = "012yzAstuvwxQRSTUVWBCDEFGHIJKLMNbcdefghijklmnOP3456789aopqrXYZ"; break;
                case "j": tabla_indice = 19; tabla = "012WXYCDEFGHIJKLMNOPQ3zAB6789abcdefghijklmnopqrstuvwxySTUVR45Z"; break;
                case "k": tabla_indice = 20; tabla = "012WXastuvwxyIJKLMNOPQ3zAB678945ZbcdefghSTUVRYCDEFGHijklmnopqr"; break;
                case "l": tabla_indice = 21; tabla = "7tFGHijk012WXasefghSTUVRYCAuvwxyIJKLMNOPQ3zbcd8945ZDEB6lmnopqr"; break;
            }
            var indice_real = tabla_indice * 62;
            int pos = 0;
            for (var n = 0; n < tabla.Length; n++)
            {
                if (item[0].Equals(tabla[n])) pos = n;
            }
            int resultado = indice_real + pos;
            return resultado;
        }
        public Boolean verificarMay(string letra)
        {
            var str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var resultado = false;
            for (var i = 0; i < str.Length; i++)
            {
                if (letra[0].Equals(str[i]))
                {
                    resultado = true;
                    break;
                }
            }
            return resultado;
        }
        /**ALGORITMO MATEMATICO: ENCRIPTACION y DESENCRIPTACION */
        public String run_desencr(String codigo)
        {
            int constante = p;
            String cedula = "";
            for (int i = 0; i < codigo.Length; i++)
            {
                int a, b, u;
                int numeroObtenido = obtenerNumero(codigo[i]);
                //Console.WriteLine(numeroObtenido);
                if ((numeroObtenido - k) < 0)
                {
                    a = absoluto(numeroObtenido - k, constante);
                    b = modulo_inverso(l, constante);
                    u = modulo(a * b, constante);
                }
                else
                {
                    a = modulo(numeroObtenido - k, constante);
                    b = modulo_inverso(l, constante);
                    u = a * b;
                }
                cedula += modulo(u, constante);
            }
            return cedula;
        }
        /**FUNCIONES DE TRANSFORMACION: LETRAS O NUMEROS*/
        public int obtenerNumero(char letra)
        {
            String str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int resultado = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (letra.Equals(str[i]))
                {
                    resultado = i;
                }
            }
            return resultado;
        }
        public int absoluto(int resta, int mod)
        {
            var respuesta = 0;
            for (int i = 0; i < mod; i++)
            {
                if (modulo(i - resta, mod) == 0)
                {
                    respuesta = i;
                }
            }
            return respuesta;
        }
        public int modulo(int num1, int mod)
        {
            return num1 % mod;
        }
        public int modulo_inverso(int num1, int mod)
        {
            int i = 0;
            int aux = 0;
            do
            {
                aux = modulo(num1 * i, mod);
                i++;
            }
            while (aux != 1);
            return (i - 1);
        }
        /*Funciones para cambiar una letra de acuerdo a la tabla de transformacion
        (getTablaTransformacion(k))*/
        public string cambio_letra_inversa(string letra)
        {
            var normal = "abcdefghijklmnopqrstuvwxyz";
            var transformada = getTablaTransformacion(k);
            var pos = 0;
            for (var n = 0; n < transformada.Length; n++)
            {
                if (letra[0].Equals(transformada[n])) pos = n;
            }
            return normal[pos] + "";//Se retorna a partir del abecedario
        }
        /*Obtener la tabla de transformacion de acuerdo al k ingresado*/
        public string getTablaTransformacion(int k_var)
        {
            var transformada = "";
            switch (k_var % 10)
            {
                //                   abcdefghijklmnopqrstuvwxyz
                case 0: transformada = "yzuvwxqrstmnopijklefghabcd"; break;
                case 1: transformada = "cdhabefgjklopitmnqrsvwxyzu"; break;
                case 2: transformada = "pmvozibktdxwfejgynulsqahrc"; break;
                case 3: transformada = "bahdckjgfetipolsrqnmzyxwvu"; break;
                case 4: transformada = "vunmzyxwpolsrqjgfetibahdck"; break;
                case 5: transformada = "dcktibahrqgjfexwpolsvunmzy"; break;
                case 6: transformada = "zynmvulspoxwfejgrqahibktdc"; break;
                case 7: transformada = "ktdcahibfejgrqlspoxwzynmvu"; break;
                case 8: transformada = "mvuxwzynqlspfejgroktdcahib"; break;
                case 9: transformada = "hibktdcagrqlspoynfejmvuxwz"; break;
            }
            return transformada;
        }

        //Transformacion desde base 32
        private int determinar_numero(string letra)
        {
            int pos = 0;
            string codigo = "0123456789abcdefghijklmnopqrstuv";
            for (var h = 0; h < codigo.Length; h++)
            {
                if (letra[0].Equals(codigo[h])) pos = h;
            }
            return pos;
        }
        /*Funcion para obtener el un unico digito en base 36 a base 10*/
        private int determinar_numeroB36(string letra)
        {
            int pos = 0;
            string codigo = "0123456789abcdefghijklmnopqrstuvwxyz";
            for (var h = 0; h < codigo.Length; h++)
            {
                if (letra[0].Equals(codigo[h])) pos = h;
            }
            return pos;
        }
        /*Para cedulas y rucs naturales*/
        private int calcularDigito10(string cedula)
        {
            int[] coeficientes = new int[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int[] digitos = new int[9];
            int[] multiplos = new int[9];
            for (var i = 0; i < cedula.Length; i++)
            {
                digitos[i] = Int32.Parse(cedula[i] + "");
                multiplos[i] = (digitos[i] * coeficientes[i]);
            }
            int suma = 0;
            for (var j = 0; j < multiplos.Length; j++)
            {
                if (multiplos[j] >= 10)
                {
                    var num = multiplos[j] + "";
                    var d1 = Int32.Parse(num[0] + "");
                    var d2 = Int32.Parse(num[1] + "");
                    multiplos[j] = d1 + d2;
                }
                suma += multiplos[j];
            }
            return 10 - (suma % 10);
        }
        private bool esNumero(string c)
        {
            int n;
            bool isNumeric = int.TryParse(c, out n);
            return isNumeric;
        }
        //Para R.U.C. JURIDICOS Y EXTRANJEROS SIN CEDULA
        private int calcularDigito10ruc9(string ruc)
        {
            int[] coeficientes = new int[] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            int[] digitos = new int[9];
            int[] multiplos = new int[9];
            for (var i = 0; i < ruc.Length; i++)
            {
                digitos[i] = Int32.Parse(ruc[i] + "");
                multiplos[i] = (digitos[i] * coeficientes[i]);
            }
            int suma = 0;
            for (var j = 0; j < multiplos.Length; j++)
            {
                suma += multiplos[j];
            }
            int residuo = suma % 11;
            return 11 - residuo;
        }
        /*Para R.U.C. PUBLICOS*/
        private int calcularDigito10ruc6(string ruc)
        {
            int[] coeficientes = new int[] { 3, 2, 7, 6, 5, 4, 3, 2 };
            int[] digitos = new int[8];
            int[] multiplos = new int[8];
            for (var i = 0; i < ruc.Length; i++)
            {
                digitos[i] = Int32.Parse(ruc[i] + "");
                multiplos[i] = (digitos[i] * coeficientes[i]);
            }
            int suma = 0;
            for (var j = 0; j < multiplos.Length; j++)
            {
                suma += multiplos[j];
            }
            int residuo = suma % 11;
            return 11 - residuo;
        }

        private string[] dividirPasaporte(string pasaporte)
        {
            string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strN = "0123456789";
            string parte_letras = "";
            string parte_numeros = "";
            string[] result = new string[2];
            for (int m = 0; m < pasaporte.Length; m++)
            {
                for (int n = 0; n < str.Length; n++)
                {
                    if (pasaporte[m] == str[n])
                    {
                        parte_letras += pasaporte[m];
                        break;
                    }
                }
            }
            for (int m = 0; m < pasaporte.Length; m++)
            {
                for (int n = 0; n < strN.Length; n++)
                {
                    if (pasaporte[m] == strN[n])
                    {
                        parte_numeros += pasaporte[m];
                        break;
                    }
                }
            }

            result[0] = parte_letras;
            result[1] = parte_numeros;

            return result;
        }
    }

}
