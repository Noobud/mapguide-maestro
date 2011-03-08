﻿#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.MapDefinition;

namespace Maestro.Editors.MapDefinition
{
    /// <summary>
    /// Editor control for Map Definitions
    /// </summary>
    public partial class MapDefinitionEditorCtrl : EditorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDefinitionEditorCtrl"/> class.
        /// </summary>
        public MapDefinitionEditorCtrl()
        {
            InitializeComponent();
        }

        private IMapDefinition _map;
        private IEditorService _edSvc;

        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            _edSvc = service;
            _map = _edSvc.GetEditedResource() as IMapDefinition;

            mapSettingsCtrl.Bind(service);
            mapLayersCtrl.Bind(service);

            mapLayersCtrl.RequestLayerOpen += new OpenLayerEventHandler(OnRequestLayerOpen);
        }

        void OnRequestLayerOpen(object sender, string layerResourceId)
        {
            _edSvc.OpenResource(layerResourceId);
        }
    }
}
