﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WebScraper.Core.Enums
{
	public static class EnumExtensions
	{
		public static string GetDescription(this JobPortals val)
		{
			DescriptionAttribute[] attributes = (DescriptionAttribute[])val
				.GetType()
				.GetField(val.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false);
			return attributes.Length > 0 ? attributes[0].Description : string.Empty;
		}
	}
}