﻿using System.ComponentModel;

namespace Tizen.NUI
{
    /// <summary>
    /// The Dp Extension class is density independent pixel(dp) unit coverter for float object.
    /// One dp is a virtual pixel unit that's roughly equal to one pixel
    /// on a medium-density(160dpi) screen.
    /// To convert Dp to pixel, use Pixel property or,
    /// See <see cref="Tizen.NUI.GraphicsTypeManager" /> and <see cref="Tizen.NUI.GraphicsTypeConverter" /> also.
    /// </summary>
        /// This will be public opened in tizen_next after ACR done. Before ACR, need to be hidden as inhouse API.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class GraphicsTypeExtensions
    {
        /// <summary>
        /// Converter float pixel to dp.
        /// 100.0f.ToDp() = 100.0f in 160dpi display.
        /// </summary>
        /// <param name="pixel">The float pixel unit value to be converted dp unit.</param>
        /// <returns>The float dp unit value.</returns>
        /// This will be public opened in tizen_next after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static float ToDp(this float pixel)
        {
            if (GraphicsTypeManager.Instance.TypeConverter is DpTypeConverter)
            {
                return GraphicsTypeManager.Instance.ConvertFromPixel(pixel);
            }
            else
            {
                return DpTypeConverter.Instance.ConvertFromPixel(pixel);
            }
        }

        /// <summary>
        /// Converter float dp to pixel.
        /// 100.0f.ToPixel() = 100.0f in 160dpi display.
        /// </summary>
        /// <param name="dp">The float dp unit value to be converted pixel unit.</param>
        /// <returns>The float pixel unit value.</returns>
        /// This will be public opened in tizen_next after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static float ToPixel(this float dp)
        {
            if (GraphicsTypeManager.Instance.TypeConverter is DpTypeConverter)
            {
                return GraphicsTypeManager.Instance.ConvertToPixel(dp);
            }
            else
            {
                return DpTypeConverter.Instance.ConvertToPixel(dp);
            }
        }

        /// <summary>
        /// Converter int pixel to dp.
        /// 100.0f.ToDp() = 100.0f in 160dpi display.
        /// </summary>
        /// <param name="pixel">The int pixel unit value to be converted dp unit.</param>
        /// <returns>The int dp unit value.</returns>
        /// This will be public opened in tizen_next after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int ToDp(this int pixel)
        {
            if (GraphicsTypeManager.Instance.TypeConverter is DpTypeConverter)
            {
                return (int)GraphicsTypeManager.Instance.ConvertFromPixel(pixel);
            }
            else
            {
                return (int)DpTypeConverter.Instance.ConvertFromPixel(pixel);
            }
        }

        /// <summary>
        /// Converter int dp to pixel.
        /// 100.0f.ToPixel() = 100.0f in 160dpi display.
        /// </summary>
        /// <param name="dp">The int dp unit value to be converted pixel unit.</param>
        /// <returns>The int pixel unit value.</returns>
        /// This will be public opened in tizen_next after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int ToPixel(this int dp)
        {
            if (GraphicsTypeManager.Instance.TypeConverter is DpTypeConverter)
            {
                return (int)GraphicsTypeManager.Instance.ConvertToPixel(dp);
            }
            else
            {
                return (int)DpTypeConverter.Instance.ConvertToPixel(dp);
            }
        }
    }
}
