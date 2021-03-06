﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System.Diagnostics;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource
{
    /// <summary>
    /// Editor control for Feature Sources
    /// </summary>
    public partial class FeatureSourceEditorCtrl : EditorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSourceEditorCtrl"/> class.
        /// </summary>
        public FeatureSourceEditorCtrl()
        {
            InitializeComponent();
        }

        private IFeatureSource _fs;

        /// <summary>
        /// Binds the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        public override void Bind(IEditorService service)
        {
            _fs = service.GetEditedResource() as IFeatureSource;
            service.RegisterCustomNotifier(this);
            Debug.Assert(_fs != null);

            CollapsiblePanel panel = FsEditorMap.GetPanel(_fs.Provider);
            var b = panel as IEditorBindable;
            if (b != null)
                b.Bind(service);

            panel.Dock = DockStyle.Top;

            var ov = new CoordSysOverrideCtrl();
            ov.Bind(service);

            ov.Dock = DockStyle.Top;

            var ext = new ExtensionsCtrl();
            ext.Bind(service);

            ext.Dock = DockStyle.Top;

            this.Controls.Add(ext);
            this.Controls.Add(ov);
            this.Controls.Add(panel);
        }
    }
}