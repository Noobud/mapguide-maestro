#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Collections.Specialized;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Collection class for building web requests to the MapGuide Server
	/// </summary>
	internal class RequestBuilder
	{
		private string m_hosturi;
		private string m_sessionID = null;
		private string m_locale = null;

		internal RequestBuilder(Uri hosturi, string locale, string sessionid)
		{
			m_hosturi = hosturi.AbsoluteUri;
			m_locale = locale;
			m_sessionID = sessionid;
		}

		internal RequestBuilder(Uri hosturi, string locale)
			: this (hosturi, locale, null)
		{
		}

		internal string Locale { get { return m_locale; } }

		internal string CreateSession()
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CREATESESSION");
			param.Add("VERSION", "1.0.0");
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			return m_hosturi + "?" + EncodeParameters(param);
		}

		internal string GetSiteVersion()
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETSITEVERSION");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			return m_hosturi + "?" + EncodeParameters(param);
		}

		internal string GetFeatureProviders()
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETFEATUREPROVIDERS");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			return m_hosturi + "?" + EncodeParameters(param);

		}

		internal string EnumerateResources(string startingpoint, int depth, string type)
		{
			if (type == null)
				type = "";

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "ENUMERATERESOURCES");
			param.Add("VERSION", "1.0.0");
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			param.Add("RESOURCEID", startingpoint);
			param.Add("DEPTH", depth.ToString());
			param.Add("TYPE", type);
			return m_hosturi + "?" + EncodeParameters(param);
		}

		internal string TestConnection(string featuresource)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "TESTCONNECTION");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			param.Add("RESOURCEID", featuresource);
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		internal System.Net.WebRequest TestConnectionPost(string providername, NameValueCollection parameters, System.IO.Stream outStream)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "TESTCONNECTION");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("PROVIDER", providername);
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach(string k in parameters)
			{
				sb.Append(k);
				sb.Append("=");
				sb.Append(parameters[k]);
				sb.Append(";");
			}
			if (sb.Length != 0)
				sb.Length = sb.Length - 1;

			//TODO: Figure out how to encode the '%' character...
			param.Add("CONNECTIONSTRING", sb.ToString());

			string boundary;
			System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
			EncodeFormParameters(boundary, param, outStream);
			req.ContentLength = outStream.Length;
			return req;
		}


		internal string TestConnection(string providername, NameValueCollection parameters)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "TESTCONNECTION");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("PROVIDER", providername);
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach(string k in parameters)
			{
				sb.Append(k);
				sb.Append("=");
				sb.Append(parameters[k]);
				sb.Append(";");
			}
			if (sb.Length != 0)
				sb.Length = sb.Length - 1;

			//TODO: Figure out how to encode the '%' character...
			param.Add("CONNECTIONSTRING", sb.ToString());

			return m_hosturi + "?" + EncodeParameters(param);
		}

		internal string SessionID
		{
			get { return m_sessionID; }
			set { m_sessionID = value; }
		}

		private string EncodeParameters(NameValueCollection param)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach(string s in param.Keys)
			{
				sb.Append(EncodeParameter(s, param[s]));
				sb.Append("&");
			}

			return sb.ToString(0, sb.Length - 1);
		}

		private string EncodeParameter(string name, string value)
		{
			return System.Web.HttpUtility.UrlEncode(name) + "=" + System.Web.HttpUtility.UrlEncode(value);
		}


		public string GetMapDWF(string id)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETMAP");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("DWFVERSION", "6.01");
			param.Add("EMAPVERSION", "1.0");
			param.Add("MAPDEFINITION", id);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string GetResourceContent(string id)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETRESOURCECONTENT");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", id);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string GetResourceData(string id, string name)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETRESOURCEDATA");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", id);
			param.Add("DATANAME", name);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string EnumerateResourceData(string id)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "ENUMERATERESOURCEDATA");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", id);

			return m_hosturi + "?" + EncodeParameters(param);
		}


		public string DeleteResourceData(string id, string name)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "DELETERESOURCEDATA");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", id);
			param.Add("DATANAME", name);

			return m_hosturi + "?" + EncodeParameters(param);
		}


		public string GetResourceHeader(string id)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETRESOURCEHEADER");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", id);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string SetResource(string id)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "SETRESOURCE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", id);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		private void EncodeFormParameters(string boundary, NameValueCollection param, System.IO.Stream responseStream)
		{
			foreach(string s in param.Keys)
			{
				System.IO.MemoryStream content = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(param[s]));
				AppendFormContent(s, null, boundary, responseStream, content);
			}
		}

		/// <summary>
		/// Writes a value/file to a form-multipart HttpRequest stream
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		/// <param name="filename">The name of the file uploaded, set to null if this parameter is not a file</param>
		/// <param name="boundary">The request boundary string</param>
		/// <param name="responseStream">The stream to write to</param>
		/// <param name="dataStream">The stream to read from. When using non-file parameters, use UTF8 encoding</param>
		private void AppendFormContent(string name, string filename, string boundary, System.IO.Stream responseStream, System.IO.Stream dataStream)
		{
			string contenttype;
			if (filename == null)
			{
				filename = "";
				contenttype = "";
			}
			else
			{
				filename = " filename=\"" + filename + "\"";
				contenttype = "\r\nContent-Type: application/octet-stream";
			}

			byte[] headers = System.Text.Encoding.UTF8.GetBytes(string.Concat(new String[] { "Content-Disposition: form-data; name=\"" + name + "\";" + filename , "\"", contenttype, "\r\n\r\n"}));
			responseStream.Write(headers, 0, headers.Length);
			
			byte[] buffer = new byte[1024 * 8];
			int r;

			do
			{
				r = dataStream.Read(buffer, 0, headers.Length);
				responseStream.Write(buffer, 0, r);
			} while (r > 0);

			byte[] footer =  System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
			responseStream.Write(footer, 0, footer.Length);
		}

		public System.Net.WebRequest SetResource(string id, System.IO.Stream outStream, System.IO.Stream content, System.IO.Stream header)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "SETRESOURCE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", id);

			string boundary;
			System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);

			EncodeFormParameters(boundary, param, outStream);
			if (content != null)
				AppendFormContent("CONTENT", "content.xml", boundary, outStream, content);					
			if (header != null)
				AppendFormContent("HEADER", "header.xml", boundary, outStream, header);

			req.ContentLength = outStream.Length;
			return req;
		}


		public System.Net.WebRequest SetResourceData(string id, string dataname, ResourceDataType datatype, System.IO.Stream outStream, System.IO.Stream content)
		{
			if (m_sessionID == null)
				throw new Exception("Connection is not yet logged in");

			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "SETRESOURCEDATA");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", id);
			param.Add("DATANAME", dataname);
			param.Add("DATATYPE", datatype.ToString());
			param.Add("DATALENGTH", content.Length.ToString());

			string boundary;
			System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);

			EncodeFormParameters(boundary, param, outStream);
			AppendFormContent("DATA", "content.bin", boundary, outStream, content);					

			req.ContentLength = outStream.Length;
			return req;
		}

		internal string reqAsUrl(string resourceId, string classname, string filter, string[] columns)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "SELECTFEATURES");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", resourceId);
			param.Add("CLASSNAME", classname);
			
			//Using the very standard TAB character for column seperation
			//  ... nice if your data has "," or ";" in the column names :)
			if (columns != null)
				param.Add("PROPERTIES", string.Join("\t", columns));

			//param.Add("COMPUTED_ALIASES", computed_aliases);
			//param.Add("COMPUTED_PROPERTIES", computed_properties);
			if (filter != null)
				param.Add("FILTER", filter);
			return m_hosturi + "?" + EncodeParameters(param);
		}

		public System.Net.WebRequest SelectFeatures(string resourceId, string classname, string filter, string[] columns, System.IO.Stream outStream)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "SELECTFEATURES");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", resourceId);
			param.Add("CLASSNAME", classname);
			
			//Using the very standard TAB character for column seperation
			//  ... nice if your data has "," or ";" in the column names :)
			if (columns != null)
				param.Add("PROPERTIES", string.Join("\t", columns));

			//param.Add("COMPUTED_ALIASES", computed_aliases);
			//param.Add("COMPUTED_PROPERTIES", computed_properties);
			if (filter != null)
				param.Add("FILTER", filter);

			string boundary;
			System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
			EncodeFormParameters(boundary, param, outStream);
			req.ContentLength = outStream.Length;

			return req;
		}

		private System.Net.WebRequest PrepareFormContent(System.IO.Stream outStream, out string boundary)
		{
			System.Net.WebRequest req = System.Net.WebRequest.Create(m_hosturi);
			boundary = "---------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] initialBound = System.Text.Encoding.UTF8.GetBytes(string.Concat("--", boundary, "\r\n"));
			req.ContentType = "multipart/form-data; boundary=" + boundary;
			req.Timeout = 20 * 1000;
			req.Method = "POST";
			outStream.Write(initialBound, 0, initialBound.Length);
			return req;
		}

		public string DescribeSchema(string resourceId, string schema)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "DESCRIBEFEATURESCHEMA");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", resourceId);
			param.Add("SCHEMA", schema);
			
			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string GetProviderCapabilities(string provider)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETPROVIDERCAPABILITIES");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("PROVIDER", provider);

			return m_hosturi + "?" + EncodeParameters(param);
		}


		public string EnumerateCategories()
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CS.ENUMERATECATEGORIES");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			return m_hosturi + "?" + EncodeParameters(param);
		}


		public string EnumerateCoordinateSystems(string category)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CS.ENUMERATECOORDINATESYSTEMS");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("CSCATEGORY", category);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string ConvertWktToCoordinateSystemCode(string wkt)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CS.CONVERTWKTTOCOORDINATESYSTEMCODE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("CSWKT", wkt);

			return m_hosturi + "?" + EncodeParameters(param);
		}


		public string ConvertCoordinateSystemCodeToWkt(string code)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CS.CONVERTCOORDINATESYSTEMCODETOWKT");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("CSCODE", code);

			return m_hosturi + "?" + EncodeParameters(param);
		}


		public string ConvertWktToEpsgCode(string wkt)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CS.CONVERTWKTTOEPSGCODE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("CSWKT", wkt);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string ConvertEpsgCodeToWkt(string code)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CS.CONVERTEPSGCODETOWKT");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("CSCODE", code);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string GetBaseLibrary()
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CS.GETBASELIBRARY");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string IsValidCoordSys(string wkt)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "CS.ISVALID");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("CSWKT", wkt);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string DeleteResource(string resourceid)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "DELETERESOURCE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", resourceid);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string MoveResource(string source, string target, bool overwrite)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "MOVERESOURCE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("SOURCE", source);
			param.Add("DESTINATION", target);
			param.Add("OVERWRITE", overwrite ? "1" : "0");

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string CopyResource(string source, string target, bool overwrite)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "COPYRESOURCE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("SOURCE", source);
			param.Add("DESTINATION", target);
			param.Add("OVERWRITE", overwrite ? "1" : "0");

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string EnumerateResourceReferences(string resourceid)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "ENUMERATERESOURCEREFERENCES");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			param.Add("RESOURCEID", resourceid);

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public System.Net.WebRequest GetMapImage(string mapname, string format, string selectionXml, double centerX, double centerY, double scale, int dpi, int width, int height, string[] showlayers, string[] hidelayers, string[] showgroups, string[] hidegroups, System.IO.Stream outStream)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETMAPIMAGE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("MAPNAME", mapname);

			if (format != null && format.Length != 0)
				param.Add("FORMAT", format);

			if (selectionXml != null && selectionXml.Length != 0)
				param.Add("SELECTION", selectionXml);
			
			param.Add("SETVIEWCENTERX", Utility.SerializeDigit(centerX));
			param.Add("SETVIEWCENTERY", Utility.SerializeDigit(centerY));
			param.Add("SETVIEWSCALE", Utility.SerializeDigit(scale));
			param.Add("SETDISPLAYDPI", dpi.ToString());
			param.Add("SETDISPLAYWIDTH", width.ToString());
			param.Add("SETDISPLAYHEIGHT", height.ToString());

			if (showlayers != null && showlayers.Length > 0)
				param.Add("SHOWLAYERS", string.Join(",", showlayers));
			if (hidelayers != null && hidelayers.Length > 0)
				param.Add("HIDELAYERS", string.Join(",", hidelayers));
			if (showgroups != null && showgroups.Length > 0)
				param.Add("SHOWGROUPS", string.Join(",", showgroups));
			if (hidegroups != null && hidegroups.Length > 0)
				param.Add("HIDEGROUPS", string.Join(",", hidegroups));

			//TODO: Find out if these actually work...
			//param.Add("SETDATAEXTENT", ...)
			//param.Add("REFRESHLAYERS", ...)

			string boundary;
			System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
			EncodeFormParameters(boundary, param, outStream);
			req.ContentLength = outStream.Length;

			return req;
		}

		public string BuildRequest(NameValueCollection param)
		{
			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string GetMapImageUrl(string mapname, string format, string selectionXml, double centerX, double centerY, double scale, int dpi, int width, int height, string[] showlayers, string[] hidelayers, string[] showgroups, string[] hidegroups)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETMAPIMAGE");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("MAPNAME", mapname);
			
			if (format != null && format.Length != 0)
				param.Add("FORMAT", format);

			if (selectionXml != null && selectionXml.Length != 0)
				param.Add("SELECTION", selectionXml);
			

			param.Add("SETVIEWCENTERX", Utility.SerializeDigit(centerX));
			param.Add("SETVIEWCENTERY", Utility.SerializeDigit(centerY));
			param.Add("SETVIEWSCALE", Utility.SerializeDigit(scale));
			param.Add("SETDISPLAYDPI", dpi.ToString());
			param.Add("SETDISPLAYWIDTH", width.ToString());
			param.Add("SETDISPLAYHEIGHT", height.ToString());

			if (showlayers != null && showlayers.Length > 0)
				param.Add("SHOWLAYERS", string.Join(",", showlayers));
			if (hidelayers != null && hidelayers.Length > 0)
				param.Add("HIDELAYERS", string.Join(",", hidelayers));
			if (showgroups != null && showgroups.Length > 0)
				param.Add("SHOWGROUPS", string.Join(",", showgroups));
			if (hidegroups != null && hidegroups.Length > 0)
				param.Add("HIDEGROUPS", string.Join(",", hidegroups));

			//TODO: Find out if these actually work...
			//param.Add("SETDATAEXTENT", ...)
			//param.Add("REFRESHLAYERS", ...)

			return m_hosturi + "?" + EncodeParameters(param);
		}

		public System.Net.WebRequest QueryMapFeatures(string mapname, bool persist, string geometry, System.IO.Stream outStream, QueryMapFeaturesLayerAttributes attributes)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "QUERYMAPFEATURES");
			param.Add("VERSION", "1.0.0");
			param.Add("PERSIST", persist ? "1" : "0");
			param.Add("MAPNAME", mapname);
			param.Add("SESSION", m_sessionID);
			param.Add("GEOMETRY", geometry);
			param.Add("SELECTIONVARIANT", "INTERSECTS");
			param.Add("MAXFEATURES", "-1");
			param.Add("LAYERATTRIBUTEFILTER", ((int)attributes).ToString());
			param.Add("FORMAT", "text/xml");

			if (m_locale != null)
				param.Add("LOCALE", m_locale);

			string boundary;
			System.Net.WebRequest req = PrepareFormContent(outStream, out boundary);
			EncodeFormParameters(boundary, param, outStream);
			req.ContentLength = outStream.Length;

			return req;
		}

		public string EnumerateApplicationTemplates()
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "ENUMERATEAPPLICATIONTEMPLATES");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");

			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			
			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string EnumerateApplicationWidgets()
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "ENUMERATEAPPLICATIONWIDGETS");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");

			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			
			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string EnumerateApplicationContainers()
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "ENUMERATEAPPLICATIONCONTAINERS");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");

			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			
			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string GetSpatialContextInfo(string resourceID, bool activeOnly)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETSPATIALCONTEXTS");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			param.Add("RESOURCEID", resourceID);
			param.Add("ACTIVEONLY", activeOnly ? "0" : "1");

			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			
			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string HostURI { get { return m_hosturi; } }

		public string GetIdentityProperties(string resourceID, string schema, string classname)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "GETIDENTITYPROPERTIES");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			param.Add("RESOURCEID", resourceID);
			param.Add("SCHEMA", schema);
			param.Add("CLASSNAME", classname);

			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			
			return m_hosturi + "?" + EncodeParameters(param);
		}

		public string EnumerateUnmanagedData(string startpath, string filter, bool recursive, UnmanagedDataTypes type)
		{
			NameValueCollection param = new NameValueCollection();
			param.Add("OPERATION", "ENUMERATEUNMANAGEDDATA");
			param.Add("VERSION", "1.0.0");
			param.Add("SESSION", m_sessionID);
			param.Add("FORMAT", "text/xml");
			if (startpath != null)
				param.Add("PATH", startpath);
			if (filter != null)
				param.Add("FILTER", filter);
			param.Add("RECURSIVE", recursive ? "1" : "0");
			if (type == UnmanagedDataTypes.Files)
				param.Add("Type", "Files");
			else if (type == UnmanagedDataTypes.Folders)
				param.Add("Type", "Folders");
			else
				param.Add("Type", "Both");

			if (m_locale != null)
				param.Add("LOCALE", m_locale);
			
			return m_hosturi + "?" + EncodeParameters(param);
		}
	}
}