using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CefSharp;
using CefSharp.Enums;

namespace Eft.Launcher.Gui.Wpf.CefEngine;

public class DragHandler : IDragHandler
{
	private readonly Action<Region> _E000;

	public DragHandler(Action<Region> callback)
	{
		_E000 = callback ?? throw new ArgumentNullException(_E05B._E000(31222));
	}

	bool IDragHandler.OnDragEnter(IWebBrowser chromiumWebBrowser, IBrowser browser, IDragData dragData, DragOperationsMask mask)
	{
		return false;
	}

	void IDragHandler.OnDraggableRegionsChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IList<DraggableRegion> regions)
	{
		if (regions == null || regions.Count <= 0 || browser.IsPopup)
		{
			return;
		}
		Region region = new Region(new Rectangle(0, 0, 0, 0));
		foreach (DraggableRegion item in regions.OrderByDescending((DraggableRegion r) => r.Draggable))
		{
			Rectangle rect = new Rectangle(item.X, item.Y, item.Width, item.Height);
			if (item.Draggable)
			{
				region.Union(rect);
			}
			else
			{
				region.Exclude(rect);
			}
		}
		_E000(region);
	}
}
