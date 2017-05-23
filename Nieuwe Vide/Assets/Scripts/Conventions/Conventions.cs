using System;
using UnityEngine;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

namespace ProefProeve.CodeConventions
{
    public class Conventions
    {
        public const int NAME_SECONDNAME = 0;
        public static object NameSecondName;

        private delegate void NameEventHandler();
        private event NameEventHandler NameEvent;

        protected object name;
        [ExampleAttribute] private object _name;
        private bool _condition;

        //Alleen properties & methods mogen public zijn, geen fields.
        public object Name
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        //Onze methods zullen geschreven worden met PascalCasing verwoord.
        //Daarbij hebben wij de brackets onder de method.
        //De methods gaan van boven naar beneden waarbij public boven staat en private onder staat.
        //Attributes boven de functie

        //Comment je code duidelijk en niet te veel text.
        //Maak gebruik van Summaries door middel van /// in je IDE.

        /// <summary>
        /// Description of method.
        /// </summary>
        public void PublicVoid()
        {

        }

        protected void ProtectedVoid()
        {

        }

        [ExampleAttribute]
        private void PrivateVoid()
        {
            if (!_condition)
                return;

            if (_condition && Input.GetKey(KeyCode.Space))
            {
                ProtectedVoid();
                int i = 2;
                i++;
            }
        }

    }
       // If statements worden aangegeven als hieronder.



    namespace ProefProeve
    {

    }

    //Groeperingen gaan als volgt
    //AI = Artificial Intelligence
    namespace ProefProeve.AI
    {

    }

    //Alle Types gebruiken PascalCasing.
    public class ClassName
    {

    }

    public struct StructName
    {

    }
    

    class ExampleAttribute : Attribute { }
}