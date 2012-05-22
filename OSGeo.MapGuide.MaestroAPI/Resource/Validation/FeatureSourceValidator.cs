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
using System.Text;

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Validation
{
    /// <summary>
    /// Resource validator for Feature Sources
    /// </summary>
    public class FeatureSourceValidator : IResourceValidator
    {
        /// <summary>
        /// Validats the specified resources for common issues associated with this
        /// resource type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resource"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public ValidationIssue[] Validate(ResourceValidationContext context, IResource resource, bool recurse)
        {
            Check.NotNull(context, "context");

            if (context.IsAlreadyValidated(resource.ResourceID))
                return null;

            if (resource.ResourceType != ResourceTypes.FeatureSource)
                return null;

            List<ValidationIssue> issues = new List<ValidationIssue>();

            IFeatureSource feature = (IFeatureSource)resource;
            IFeatureService featSvc = feature.CurrentConnection.FeatureService;
            //Note: Must be saved!
            string s = featSvc.TestConnection(feature.ResourceID);
            if (s.Trim().ToUpper() != true.ToString().ToUpper())
                return new ValidationIssue[] { new ValidationIssue(feature, ValidationStatus.Error, ValidationStatusCode.Error_FeatureSource_ConnectionTestFailed, string.Format(Properties.Resources.FS_ConnectionTestFailed, s)) };

            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;
                FdoSpatialContextList lst = context.GetSpatialContexts(feature.ResourceID);
                if (lst == null || lst.SpatialContext == null || lst.SpatialContext.Count == 0)
                    issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, ValidationStatusCode.Warning_FeatureSource_NoSpatialContext, Properties.Resources.FS_NoSpatialContextWarning));
                else
                    foreach (FdoSpatialContextListSpatialContext c in lst.SpatialContext)
                        if (c.Extent == null || c.Extent.LowerLeftCoordinate == null || c.Extent.UpperRightCoordinate == null)
                            issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, ValidationStatusCode.Warning_FeatureSource_EmptySpatialContext, Properties.Resources.FS_EmptySpatialContextWarning));
                        else if (double.Parse(c.Extent.LowerLeftCoordinate.X, ci) <= -1000000 && double.Parse(c.Extent.LowerLeftCoordinate.Y, ci) <= -1000000 && double.Parse(c.Extent.UpperRightCoordinate.X, ci) >= 1000000 && double.Parse(c.Extent.UpperRightCoordinate.Y, ci) >= 1000000)
                            issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, ValidationStatusCode.Warning_FeatureSource_DefaultSpatialContext, Properties.Resources.FS_DefaultSpatialContextWarning));
            }
            catch (Exception ex)
            {
                string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                issues.Add(new ValidationIssue(feature, ValidationStatus.Error, ValidationStatusCode.Error_FeatureSource_SpatialContextReadError, string.Format(Properties.Resources.FS_SpatialContextReadError, msg)));
            }

            List<string> classes = new List<string>();
            try
            {
                var schemaNames = featSvc.GetSchemas(feature.ResourceID);
                if (schemaNames.Length == 0)
                    issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, ValidationStatusCode.Warning_FeatureSource_NoSchemasFound, Properties.Resources.FS_SchemasMissingWarning));
            }
            catch (Exception ex)
            {
                var wex = ex as System.Net.WebException;
                if (wex != null) //Most likely timeout due to really large schema
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    issues.Add(new ValidationIssue(feature, ValidationStatus.Warning, ValidationStatusCode.Warning_FeatureSource_Validation_Timeout, string.Format(Properties.Resources.FS_ValidationTimeout, msg)));
                }
                else
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    issues.Add(new ValidationIssue(feature, ValidationStatus.Error, ValidationStatusCode.Error_FeatureSource_SchemaReadError, string.Format(Properties.Resources.FS_SchemaReadError, msg)));
                }
            }

            var classNames = featSvc.GetClassNames(feature.ResourceID, null);
            foreach (var className in classNames)
            {
                try 
                {
                    string[] idProps = featSvc.GetIdentityProperties(feature.ResourceID, className);
                    if (idProps.Length == 0)
                        issues.Add(new ValidationIssue(feature, ValidationStatus.Information, ValidationStatusCode.Info_FeatureSource_NoPrimaryKey, string.Format(Properties.Resources.FS_PrimaryKeyMissingInformation, className)));
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    if (msg.Contains("MgClassNotFound")) //#1403 workaround
                        issues.Add(new ValidationIssue(feature, ValidationStatus.Information, ValidationStatusCode.Info_FeatureSource_NoPrimaryKey, string.Format(Properties.Resources.FS_PrimaryKeyMissingInformation, className)));
                    else
                        issues.Add(new ValidationIssue(feature, ValidationStatus.Error, ValidationStatusCode.Error_FeatureSource_SchemaReadError, string.Format(Properties.Resources.FS_SchemaReadError, msg)));
                }
            }
            context.MarkValidated(resource.ResourceID);

            return issues.ToArray();
        }

        /// <summary>
        /// Gets the resource type and version this validator supports
        /// </summary>
        /// <value></value>
        public ResourceTypeDescriptor SupportedResourceAndVersion
        {
            get { return new ResourceTypeDescriptor(ResourceTypes.FeatureSource, "1.0.0"); }
        }
    }
}
