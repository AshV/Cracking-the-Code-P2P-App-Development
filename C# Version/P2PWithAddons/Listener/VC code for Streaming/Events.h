#ifndef __EVENT_H__
#define __EVENT_H__

#include <atlbase.h>
#include <atlcom.h>

#include "wmencode.h"
#include "wmsencid.h"
//#include "BroadcastIt.h"
class CBroadcastIt;

#define EVENT_ID        100
#define LIB_VERMAJOR    1
#define LIB_VERMINOR    0

class CEvents : public IDispEventImpl< EVENT_ID, 
                                         CEvents,
                                         &DIID__IWMEncoderEvents,
                                         &LIBID_WMEncoderLib,
                                         LIB_VERMAJOR, 
                                         LIB_VERMINOR >
{

public:
	int a;

    static _ATL_FUNC_INFO StateChangeInfo;
    static _ATL_FUNC_INFO ErrorInfo;
    static _ATL_FUNC_INFO ArchiveStateChangeInfo;
    static _ATL_FUNC_INFO ConfigChangeInfo;
    static _ATL_FUNC_INFO ClientConnectInfo;
    static _ATL_FUNC_INFO ClientDisconnectInfo;
    static _ATL_FUNC_INFO SourceStateChangeInfo;
    static _ATL_FUNC_INFO IndexerStateChangeInfo;

BEGIN_SINK_MAP(CEvents)
    SINK_ENTRY_INFO( EVENT_ID, 
                     DIID__IWMEncoderEvents,
                     DISPID_ENCODEREVENT_STATECHANGE,
                     OnStateChange,
                     &StateChangeInfo )
    SINK_ENTRY_INFO( EVENT_ID,
                     DIID__IWMEncoderEvents,
                     DISPID_ENCODEREVENT_ERROR,
                     OnError, 
                     &ErrorInfo )
    SINK_ENTRY_INFO( EVENT_ID,
                     DIID__IWMEncoderEvents,
                     DISPID_ENCODEREVENT_ARCHIVESTATECHANGE,
                     OnArchiveStateChange,
                     &ArchiveStateChangeInfo )
    SINK_ENTRY_INFO( EVENT_ID,
                     DIID__IWMEncoderEvents,
                     DISPID_ENCODEREVENT_CONFIGCHANGE,
                     OnConfigChange,
                     &ConfigChangeInfo)
    SINK_ENTRY_INFO( EVENT_ID,
                     DIID__IWMEncoderEvents,
                     DISPID_ENCODEREVENT_CLIENTCONNECT,
                     OnClientConnect,
                     &ClientConnectInfo)
    SINK_ENTRY_INFO( EVENT_ID,
                     DIID__IWMEncoderEvents,
                     DISPID_ENCODEREVENT_CLIENTDISCONNECT,
                     OnClientDisconnect,
                     &ClientDisconnectInfo)
    SINK_ENTRY_INFO( EVENT_ID,
                     DIID__IWMEncoderEvents,
                     DISPID_ENCODEREVENT_SRCSTATECHANGE,
                     OnSourceStateChange,
                     &SourceStateChangeInfo )
    SINK_ENTRY_INFO( EVENT_ID,
                     DIID__IWMEncoderEvents,
                     DISPID_ENCODEREVENT_INDEXERSTATECHANGE,
                     OnIndexerStateChange,
                     &IndexerStateChangeInfo )
END_SINK_MAP()

public:
    STDMETHOD(OnStateChange)(/*[in]*/ WMENC_ENCODER_STATE enumState);
    STDMETHOD(OnSourceStateChange)(
                 /*[in]*/WMENC_SOURCE_STATE enumState,
                 /*[in*/WMENC_SOURCE_TYPE enumType,
                 /*[in*/short iIndex,
                 /*[in]*/BSTR bstrSourceGroup);
    STDMETHOD(OnError)(/*[in]*/ long hResult);
    STDMETHOD(OnArchiveStateChange)(
                 /*[in]*/ WMENC_ARCHIVE_TYPE enumArchive,
                 /*[in]*/ WMENC_ARCHIVE_STATE enumState );
    STDMETHOD(OnConfigChange)(/*[in]*/ long hResult, /*[in]*/ BSTR bstr);
    STDMETHOD(OnClientConnect)(
                 /*[in]*/ WMENC_BROADCAST_PROTOCOL protocol,
                 /*[in]*/ BSTR bstr);
    STDMETHOD(OnClientDisconnect)(
                 /*[in]*/ WMENC_BROADCAST_PROTOCOL protocol,
                 /*[in]*/ BSTR bstr);   
    STDMETHOD(OnIndexerStateChange)(
                 /*[in]*/ WMENC_INDEXER_STATE enumIndexerState,
                 /*[in]*/ BSTR bstrFile );

    HRESULT CEvents::Init( IWMEncoder* pEncoder )
    {
        HRESULT hr = DispEventAdvise( pEncoder );
        if( FAILED( hr ) ) 
        {
        }
    return hr;
    }

    HRESULT CEvents::ShutDown( IWMEncoder* pEncoder )
    {
        HRESULT hr = DispEventUnadvise( pEncoder );
        if( FAILED( hr ) ) 
        {
        }
        return hr;
    }

    CEvents(CBroadcastIt* br);
	CBroadcastIt* broadcast;

};

/////////////////////////////////////////////////////////////

CEvents::CEvents(CBroadcastIt* br)
{
	broadcast = br;
	a = 0;
}


_ATL_FUNC_INFO CEvents::ArchiveStateChangeInfo= {CC_STDCALL, 
          VT_ERROR, 2, { VT_I4, VT_I4 } };

/////////////////////////////////////////////////////////////

STDMETHODIMP CEvents::OnArchiveStateChange(
                WMENC_ARCHIVE_TYPE enumArchive, 
                WMENC_ARCHIVE_STATE enumState )
{
switch ( enumArchive )
    {
    case WMENC_ARCHIVE_LOCAL:
			broadcast->HeraldThisMessage(L"WMENC_ARCHIVE_LOCAL");
        break;
    default:
        break;    
    }

switch ( enumState )
    {
    case WMENC_ARCHIVE_RUNNING:
        // Process the case.
			broadcast->HeraldThisMessage(L"WMENC_ARCHIVE_RUNNING");
        break;

    case WMENC_ARCHIVE_PAUSED:
        // Process the case.
			broadcast->HeraldThisMessage(L"WMENC_ARCHIVE_PAUSED");
        break;
    
    case WMENC_ARCHIVE_STOPPED:
        // Process the case.
			broadcast->HeraldThisMessage(L"WMENC_ARCHIVE_STOPPED");
        break;
    
    default:
			broadcast->HeraldThisMessage(L"Archive");
        break;
    }
return E_NOTIMPL;
}


_ATL_FUNC_INFO CEvents::IndexerStateChangeInfo = {CC_STDCALL, 
         VT_ERROR, 2, { VT_I4, VT_BSTR } };

///////////////////////////////////////////////

STDMETHODIMP CEvents::OnIndexerStateChange(
                 WMENC_INDEXER_STATE enumIndexerState,
                 BSTR bstrFile )
{
			broadcast->HeraldThisMessage(L"IndexerStateChange");
    return E_NOTIMPL;
}


_ATL_FUNC_INFO CEvents::ClientDisconnectInfo = {CC_STDCALL,
            VT_ERROR, 2, { VT_I4, VT_BSTR } };

STDMETHODIMP CEvents::OnClientDisconnect(
                 WMENC_BROADCAST_PROTOCOL protocol, 
                 BSTR bstr)
{
	CComBSTR bstr1(L"Client ");
	bstr1.AppendBSTR(bstr);
	bstr1.Append("  Disconnected");

	broadcast->HeraldThisMessage(bstr1);
return E_NOTIMPL;
}

_ATL_FUNC_INFO CEvents::ClientConnectInfo = {CC_STDCALL,
          VT_ERROR, 2, { VT_I4, VT_BSTR } };

///////////////////////////////////////////////

STDMETHODIMP CEvents::OnClientConnect(
                WMENC_BROADCAST_PROTOCOL protocol, 
                BSTR bstr)
{
	CComBSTR bstr1(L"Client ");
	bstr1.AppendBSTR(bstr);
	bstr1.Append("  Connected");
	broadcast->HeraldThisMessage(bstr1);
	return E_NOTIMPL;
}


_ATL_FUNC_INFO CEvents::ConfigChangeInfo
= {CC_STDCALL, VT_ERROR, 2, { VT_I4, VT_BSTR } };

///////////////////////////////////////////////

STDMETHODIMP CEvents::OnConfigChange(long hResult, BSTR bstr)
{

	broadcast->HeraldThisMessage(L"Configuation Changed");
	broadcast->HeraldThisMessage(bstr);
return E_NOTIMPL;
}


_ATL_FUNC_INFO CEvents::ErrorInfo = {CC_STDCALL, 
         VT_ERROR, 1, { VT_I4 } };

///////////////////////////////////////////////

STDMETHODIMP CEvents::OnError(long hResult)
{

	broadcast->HeraldThisMessage(L"Error Encountered");

return E_NOTIMPL;
}


_ATL_FUNC_INFO CEvents::SourceStateChangeInfo = {CC_STDCALL, 
          VT_ERROR, 3, { VT_I4, VT_I4, VT_I2 } };

///////////////////////////////////////////////

STDMETHODIMP CEvents::OnSourceStateChange(
                 WMENC_SOURCE_STATE enumState,
                 WMENC_SOURCE_TYPE enumType,
                 short iIndex,BSTR i)
{
switch ( enumState )
    {
    case WMENC_SOURCE_START:
			
			broadcast->HeraldThisMessage(L"Started");

        break;
    case WMENC_SOURCE_STOP:

			broadcast->HeraldThisMessage(L"Stop");

        break;
    default:

			broadcast->HeraldThisMessage(L"Encoding");

        break;
    }

switch ( enumType )
    {
    case WMENC_AUDIO:

		broadcast->HeraldThisMessage(L"WMENC_AUDIO");

        break;
    case WMENC_VIDEO:

		broadcast->HeraldThisMessage(L"WMENC_VIDEO");

        break;
    case WMENC_SCRIPT:

		broadcast->HeraldThisMessage(L"WMENC_SCRIPT");

        break;
    default:

        break;
    }

return E_NOTIMPL;
}

_ATL_FUNC_INFO CEvents::StateChangeInfo = {CC_STDCALL, 
        VT_ERROR, 1, { VT_I4 } };

///////////////////////////////////////////////

STDMETHODIMP CEvents::OnStateChange(WMENC_ENCODER_STATE enumState)
{
switch ( enumState )
    {
    case WMENC_ENCODER_STARTING:

		broadcast->HeraldThisMessage(L"Encoder Starting");

        break;
	case WMENC_ENCODER_RUNNING:

		broadcast->HeraldThisMessage(L"Encoder Running");

        break;
    case WMENC_ENCODER_PAUSED:

		broadcast->HeraldThisMessage(L"Encoder Paused");

        break;
    case WMENC_ENCODER_STOPPING:

		broadcast->HeraldThisMessage(L"Encoder Stopping");

        break;
    case WMENC_ENCODER_STOPPED:

		broadcast->HeraldThisMessage(L"Encoder Stoped");

        break;
    default:

        break;
    }
return E_NOTIMPL;
}///////////////////////////////////////////////

#endif // __EVENT_H__
