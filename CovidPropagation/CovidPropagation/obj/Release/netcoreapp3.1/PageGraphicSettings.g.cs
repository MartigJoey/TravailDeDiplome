﻿#pragma checksum "..\..\..\PageGraphicSettings.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1D7E3ABA49726A2F94469D6CD24175A405CCA558"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using CovidPropagation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CovidPropagation {
    
    
    /// <summary>
    /// PageGraphicSettings
    /// </summary>
    public partial class PageGraphicSettings : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel spnMain;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdMenu;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddRow;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveRow;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddColumn;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveColumn;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddGraph;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddGUI;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollerViewer;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdContent;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\PageGraphicSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition Row1;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CovidPropagation;component/pagegraphicsettings.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\PageGraphicSettings.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.spnMain = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 2:
            this.grdMenu = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.btnAddRow = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\PageGraphicSettings.xaml"
            this.btnAddRow.Click += new System.Windows.RoutedEventHandler(this.AddRow_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnRemoveRow = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\PageGraphicSettings.xaml"
            this.btnRemoveRow.Click += new System.Windows.RoutedEventHandler(this.RemoveRow_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnAddColumn = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\PageGraphicSettings.xaml"
            this.btnAddColumn.Click += new System.Windows.RoutedEventHandler(this.AddColumn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnRemoveColumn = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\PageGraphicSettings.xaml"
            this.btnRemoveColumn.Click += new System.Windows.RoutedEventHandler(this.RemoveColumn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnAddGraph = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\PageGraphicSettings.xaml"
            this.btnAddGraph.Click += new System.Windows.RoutedEventHandler(this.AddChart_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnAddGUI = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\..\PageGraphicSettings.xaml"
            this.btnAddGUI.Click += new System.Windows.RoutedEventHandler(this.AddGUI_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.scrollerViewer = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 10:
            this.grdContent = ((System.Windows.Controls.Grid)(target));
            return;
            case 11:
            this.Row1 = ((System.Windows.Controls.RowDefinition)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
