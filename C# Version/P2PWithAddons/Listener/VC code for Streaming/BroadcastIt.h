// BroadcastIt.h : Declaration of the CBroadcastIt

#ifndef __BROADCASTIT_H_
#define __BROADCASTIT_H_

#include "resource.h"       // main symbols
#include <atlctl.h>
#include "BroadcastDllCP.h"


/////////////////////////////////////////////////////////////////////////////
// CBroadcastIt
class ATL_NO_VTABLE CBroadcastIt : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CStockPropImpl<CBroadcastIt, IBroadcastIt, &IID_IBroadcastIt, &LIBID_BROADCASTDLLLib>,
	public CComControl<CBroadcastIt>,
	public IPersistStreamInitImpl<CBroadcastIt>,
	public IOleControlImpl<CBroadcastIt>,
	public IOleObjectImpl<CBroadcastIt>,
	public IOleInPlaceActiveObjectImpl<CBroadcastIt>,
	public IViewObjectExImpl<CBroadcastIt>,
	public IOleInPlaceObjectWindowlessImpl<CBroadcastIt>,
	public ISupportErrorInfo,
	public IConnectionPointContainerImpl<CBroadcastIt>,
	public IPersistStorageImpl<CBroadcastIt>,
	public ISpecifyPropertyPagesImpl<CBroadcastIt>,
	public IQuickActivateImpl<CBroadcastIt>,
	public IDataObjectImpl<CBroadcastIt>,
	public IProvideClassInfo2Impl<&CLSID_BroadcastIt, &DIID__IBroadcastItEvents, &LIBID_BROADCASTDLLLib>,
	public IPropertyNotifySinkCP<CBroadcastIt>,
	public CComCoClass<CBroadcastIt, &CLSID_BroadcastIt>,
	public CProxy_IBroadcastItEvents< CBroadcastIt >,
	public CProxy_IConfigureEvents< CBroadcastIt >
{
public:
	CBroadcastIt()
	{
		m_bWindowOnly = TRUE;
		bSession = FALSE;
		video	=	FALSE;
		audio	=	FALSE;
		shUseDevice = 0;
		shUseScript	= 0;
		bEncoder = FALSE;
		EventAdvidsed = FALSE;
	}

DECLARE_REGISTRY_RESOURCEID(IDR_BROADCASTIT)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CBroadcastIt)
	COM_INTERFACE_ENTRY(IBroadcastIt)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IViewObjectEx)
	COM_INTERFACE_ENTRY(IViewObject2)
	COM_INTERFACE_ENTRY(IViewObject)
	COM_INTERFACE_ENTRY(IOleInPlaceObjectWindowless)
	COM_INTERFACE_ENTRY(IOleInPlaceObject)
	COM_INTERFACE_ENTRY2(IOleWindow, IOleInPlaceObjectWindowless)
	COM_INTERFACE_ENTRY(IOleInPlaceActiveObject)
	COM_INTERFACE_ENTRY(IOleControl)
	COM_INTERFACE_ENTRY(IOleObject)
	COM_INTERFACE_ENTRY(IPersistStreamInit)
	COM_INTERFACE_ENTRY2(IPersist, IPersistStreamInit)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
	COM_INTERFACE_ENTRY(IConnectionPointContainer)
	COM_INTERFACE_ENTRY(ISpecifyPropertyPages)
	COM_INTERFACE_ENTRY(IQuickActivate)
	COM_INTERFACE_ENTRY(IPersistStorage)
	COM_INTERFACE_ENTRY(IDataObject)
	COM_INTERFACE_ENTRY(IProvideClassInfo)
	COM_INTERFACE_ENTRY(IProvideClassInfo2)
	COM_INTERFACE_ENTRY_IMPL(IConnectionPointContainer)
END_COM_MAP()

BEGIN_PROP_MAP(CBroadcastIt)
	PROP_DATA_ENTRY("_cx", m_sizeExtent.cx, VT_UI4)
	PROP_DATA_ENTRY("_cy", m_sizeExtent.cy, VT_UI4)
/*	PROP_ENTRY("Appearance", DISPID_APPEARANCE, CLSID_NULL)
	PROP_ENTRY("AutoSize", DISPID_AUTOSIZE, CLSID_NULL)
	PROP_ENTRY("BackColor", DISPID_BACKCOLOR, CLSID_StockColorPage)
	PROP_ENTRY("BackStyle", DISPID_BACKSTYLE, CLSID_NULL)
	PROP_ENTRY("BorderColor", DISPID_BORDERCOLOR, CLSID_StockColorPage)
	PROP_ENTRY("BorderStyle", DISPID_BORDERSTYLE, CLSID_NULL)
	PROP_ENTRY("BorderVisible", DISPID_BORDERVISIBLE, CLSID_NULL)
	PROP_ENTRY("BorderWidth", DISPID_BORDERWIDTH, CLSID_NULL)
	PROP_ENTRY("Caption", DISPID_CAPTION, CLSID_NULL)
	PROP_ENTRY("DrawMode", DISPID_DRAWMODE, CLSID_NULL)
	PROP_ENTRY("DrawStyle", DISPID_DRAWSTYLE, CLSID_NULL)
	PROP_ENTRY("DrawWidth", DISPID_DRAWWIDTH, CLSID_NULL)
	PROP_ENTRY("Enabled", DISPID_ENABLED, CLSID_NULL)
	PROP_ENTRY("FillColor", DISPID_FILLCOLOR, CLSID_StockColorPage)
	PROP_ENTRY("FillStyle", DISPID_FILLSTYLE, CLSID_NULL)
	PROP_ENTRY("Font", DISPID_FONT, CLSID_StockFontPage)
	PROP_ENTRY("ForeColor", DISPID_FORECOLOR, CLSID_StockColorPage)
	PROP_ENTRY("HWND", DISPID_HWND, CLSID_NULL)
	PROP_ENTRY("MouseIcon", DISPID_MOUSEICON, CLSID_StockPicturePage)
	PROP_ENTRY("MousePointer", DISPID_MOUSEPOINTER, CLSID_NULL)
	PROP_ENTRY("Picture", DISPID_PICTURE, CLSID_StockPicturePage)
	PROP_ENTRY("TabStop", DISPID_TABSTOP, CLSID_NULL)
	PROP_ENTRY("Text", DISPID_TEXT, CLSID_NULL)
	PROP_ENTRY("Valid", DISPID_VALID, CLSID_NULL)*/
	//PROP_ENTRY("Configure", 1, CLSID_NULL)
	//PROP_ENTRY("Configure", 1, CLSID_Configure)
//	PROP_PAGE(CLSID_StockColorPage)
	PROP_PAGE(CLSID_Configure)

	// Example entries
	// PROP_ENTRY("Property Description", dispid, clsid)
	// PROP_PAGE(CLSID_StockColorPage)
END_PROP_MAP()

BEGIN_CONNECTION_POINT_MAP(CBroadcastIt)
	CONNECTION_POINT_ENTRY(IID_IPropertyNotifySink)
	CONNECTION_POINT_ENTRY(DIID__IBroadcastItEvents)
	CONNECTION_POINT_ENTRY(DIID__IConfigureEvents)
END_CONNECTION_POINT_MAP()

BEGIN_MSG_MAP(CBroadcastIt)
	CHAIN_MSG_MAP(CComControl<CBroadcastIt>)
	DEFAULT_REFLECTION_HANDLER()
END_MSG_MAP()
// Handler prototypes:
//  LRESULT MessageHandler(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
//  LRESULT CommandHandler(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
//  LRESULT NotifyHandler(int idCtrl, LPNMHDR pnmh, BOOL& bHandled);



// ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid)
	{
		static const IID* arr[] = 
		{
			&IID_IBroadcastIt,
		};
		for (int i=0; i<sizeof(arr)/sizeof(arr[0]); i++)
		{
			if (InlineIsEqualGUID(*arr[i], riid))
				return S_OK;
		}
		return S_FALSE;
	}

// IViewObjectEx
	DECLARE_VIEW_STATUS(VIEWSTATUS_SOLIDBKGND | VIEWSTATUS_OPAQUE)

// IBroadcastIt
public:
	STDMETHOD(InitializeBroadcaster)();
	STDMETHOD(get_UseScript)(/*[out, retval]*/ short *pVal);
	STDMETHOD(put_UseScript)(/*[in]*/ short newVal);
	STDMETHOD(PrepareToEncode)();
	STDMETHOD(SendURL)(/*[in]*/ BSTR bstrURL);
	STDMETHOD(SendScript)(/*[in]*/ BSTR bstrScript);
	STDMETHOD(get_AudioDevices)(/*[out, retval]*/ VARIANT *pVal);
	STDMETHOD(get_VideoMedia)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_VideoMedia)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_AudioMedia)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_AudioMedia)(/*[in]*/ BSTR newVal);
	STDMETHOD(Status)();
	STDMETHOD(CloseSession)();
	STDMETHOD(MakeSession)();
	STDMETHOD(get_ProfilesList)(/*[out, retval]*/ VARIANT *pVal);
	STDMETHOD(HeraldThisMessage)(/*[in]*/ BSTR bstr);
	STDMETHOD(get_Port)(/*[out, retval]*/ short *pVal);
	STDMETHOD(put_Port)(/*[in]*/ short newVal);
	STDMETHOD(get_Profile)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_Profile)(/*[in]*/BSTR newVal );
	STDMETHOD(Broadcast)();

	HRESULT OnDraw(ATL_DRAWINFO& di)
	{
		RECT& rc = *(RECT*)di.prcBounds;
		Rectangle(di.hdcDraw, rc.left, rc.top, rc.right, rc.bottom);

		SetTextAlign(di.hdcDraw, TA_CENTER|TA_BASELINE);
		LPCTSTR pszText = _T("BroadcastIt Dll © Dreamtech Softwares Inc., India Developed By Ankur Verma");
		TextOut(di.hdcDraw, 
			(rc.left + rc.right) / 2, 
			(rc.top + rc.bottom) / 2, 
			pszText, 
			lstrlen(pszText));

		return S_OK;
	}

	short shUseDevice;
	short shPortNo;
	short shUseScript;

	bool video,audio,bEncoder,EventAdvidsed;
	CComBSTR bstrAudioMedia;
	CComBSTR bstrVideoMedia;
	CComBSTR bstrProfile;

	IWMEncoder* pEncoder;
	IWMEncSourceGroupCollection* pSrcGrpColl;
	IWMEncSourceGroup* pSrcGrp;

	BOOL bSession;

	short m_nAppearance;
	OLE_COLOR m_clrBackColor;
	LONG m_nBackStyle;
	OLE_COLOR m_clrBorderColor;
	LONG m_nBorderStyle;
	BOOL m_bBorderVisible;
	LONG m_nBorderWidth;
	CComBSTR m_bstrCaption;
	LONG m_nDrawMode;
	LONG m_nDrawStyle;
	LONG m_nDrawWidth;
	BOOL m_bEnabled;
	OLE_COLOR m_clrFillColor;
	LONG m_nFillStyle;
	CComPtr<IFontDisp> m_pFont;
	OLE_COLOR m_clrForeColor;
	CComPtr<IPictureDisp> m_pMouseIcon;
	LONG m_nMousePointer;
	CComPtr<IPictureDisp> m_pPicture;
	BOOL m_bTabStop;
	CComBSTR m_bstrText;
	BOOL m_bValid;
};

#endif //__BROADCASTIT_H_
