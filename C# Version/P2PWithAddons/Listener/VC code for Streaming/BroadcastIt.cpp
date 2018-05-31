// BroadcastIt.cpp : Implementation of CBroadcastIt

#include "stdafx.h"
#include "BroadcastDll.h"
#include "BroadcastIt.h"
#include <comdef.h>
#include "Events.h"
CEvents *events;

/////////////////////////////////////////////////////////////////////////////
// CBroadcastIt

STDMETHODIMP CBroadcastIt::HeraldThisMessage(BSTR bstr)
{
	Fire_BroadcastStatus(bstr);
	return S_OK;
}


STDMETHODIMP CBroadcastIt::MakeSession()
{
	HRESULT hr;
	if(!bEncoder)
		return S_FALSE;

	if(bSession)
		return S_FALSE;

	bSession = TRUE;

	//pEncoder->Reset();

	CComPtr<IWMEncAttributes> pAttr;
	CComBSTR m_bstrName;
	long cnt;
	CComVariant m_varValue;
	CComVariant m_varIndex;
    VARIANT_BOOL* vbAutoStop = 0;
	IWMEncDisplayInfo* pDispInfo;
	CComBSTR m_bstrAuthor("Ankur Verma.");
	CComBSTR m_bstrCopyright("© 2000 Dreamtech Softwares Inc., India.");
	CComBSTR m_bstrDescription("Media Data.");
	CComBSTR m_bstrRating("");
	CComBSTR m_bstrTitle("The Dreamtech P2P Streaming Demo");

	hr = pEncoder->get_Attributes(&pAttr);

	_bstr_t bName[] = 
	{
		_bstr_t("Title: "),
		_bstr_t("Author: "),
		_bstr_t("Copyright: "),
		_bstr_t("Date Created: "),
		_bstr_t("Time Created: "),
		_bstr_t("File Content: ")
	};

	SYSTEMTIME sysTime;
	GetLocalTime(&sysTime);
	TCHAR SysDate[20]; 
	TCHAR SysTime[20];

	wsprintf(SysTime,"%d : %d : %d",sysTime.wHour,sysTime.wMinute,sysTime.wSecond);
	wsprintf(SysDate,"%d : %d : %d",sysTime.wDay,sysTime.wMonth,sysTime.wYear);
	
	USES_CONVERSION;
	Fire_BroadcastStatus(T2OLE(SysTime));
	Fire_BroadcastStatus(T2OLE(SysDate));

	_variant_t vVal[] = 
	{
		_variant_t("The P2P Media Streaming Demo"),
		_variant_t("Ankur Verma"),
		_variant_t("© Dreamtech Softwares Inc., India"),
		_variant_t(SysDate),
		_variant_t(SysTime),
		_variant_t("multimedia data"),
	};

	for(int i=0;i<6;i++)
	{
		pAttr->Add(bName[i],vVal[i]);
	}

	pAttr->get_Count(&cnt);
	TCHAR dh[100];
	for (i=0; i<cnt; i++)
	{
		pAttr->Item(i, &m_bstrName, &m_varValue);
		wsprintf(dh,"%s : %s" , OLE2T(m_bstrName),OLE2T(m_varValue.bstrVal));
		Fire_BroadcastStatus(T2OLE(dh));
	}

    pEncoder->put_AutoStop(VARIANT_TRUE);

    pEncoder->get_AutoIndex(vbAutoStop);


	hr = pEncoder->get_DisplayInfo(&pDispInfo);

	hr = pDispInfo->put_Author(m_bstrAuthor);
	hr = pDispInfo->put_Copyright(m_bstrCopyright);
	hr = pDispInfo->put_Description(m_bstrDescription);
	hr = pDispInfo->put_Rating(m_bstrRating);
	hr = pDispInfo->put_Title(m_bstrTitle);

	/*CComPtr<IWMEncSource> pScriptSrc;
	hr = pSrcGrp->AddSource(WMENC_SCRIPT, &pScriptSrc);
	CComBSTR bstrScript(L"UserScript://");
	hr = pScriptSrc->SetInput(bstrScript);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Use Script Failed");
		return S_FALSE;
	}*/
	return S_OK;
}


STDMETHODIMP CBroadcastIt::Broadcast()
{
	if(!bEncoder)
		return S_FALSE;

	HRESULT hr;

	Fire_BroadcastStatus(L"About to Start");

	hr = pEncoder->Start();
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Couldn't Start Encoder");
		return S_FALSE;
	}
	/*}
	catch(_com_error e)
	{
		Fire_BroadcastStatus(L"Error ............ ");
		char no2[10];
		itoa(e.Error(),no2,10);
		Fire_BroadcastStatus(T2OLE(_T(no2)));
		Fire_BroadcastStatus(T2OLE(e.ErrorMessage()));
		Fire_BroadcastStatus(L"Error ............ ");
	}*/

	Fire_BroadcastStatus(L"Copy Complete");
	if(!EventAdvidsed)
	{
		events = new CEvents(this);
		events->Init(pEncoder);
		EventAdvidsed = TRUE;
	}
 	return S_OK;
}


STDMETHODIMP CBroadcastIt::CloseSession()
{
	if(!bEncoder)
		return S_FALSE;

	if(bSession)
	{
		HRESULT hr;

		short shACount, shVCount,shSCount;
		CComVariant varIndex;
		varIndex.vt = VT_I2;
		varIndex.iVal = 0;

		hr = pSrcGrp->PrepareToEncode(VARIANT_FALSE);
		if(FAILED(hr))
			Fire_BroadcastStatus(L"Prepare encode failed from Source Group");

    			
		hr = pSrcGrp->get_SourceCount(WMENC_AUDIO, &shACount);
		if(FAILED(hr))
			Fire_BroadcastStatus(L"Getting Audio Source count failed");
		else
			if (shACount != 0)
				hr = pSrcGrp->RemoveSource(WMENC_AUDIO, varIndex);

		hr = pSrcGrp->get_SourceCount(WMENC_VIDEO, &shVCount);
		if(FAILED(hr))
			Fire_BroadcastStatus(L"Getting Vedio Source count failed");
		else
			if (shVCount != 0)
				hr = pSrcGrp->RemoveSource(WMENC_VIDEO, varIndex);

		hr = pSrcGrp->get_SourceCount(WMENC_VIDEO, &shSCount);
		if(FAILED(hr))
			Fire_BroadcastStatus(L"Getting Script Source count failed");
		else
			if (shSCount != 0)
				hr = pSrcGrp->RemoveSource(WMENC_SCRIPT, varIndex);


	    hr = pSrcGrpColl->Remove(varIndex);
		if(FAILED(hr))
			Fire_BroadcastStatus(L"Remove soruce group failed");

		hr = pEncoder->PrepareToEncode(VARIANT_FALSE);
		if(FAILED(hr))
			Fire_BroadcastStatus(L"Prepare encode failed");

		hr = pEncoder->Stop();
		if(FAILED(hr))
			Fire_BroadcastStatus(L"Couldn't Stop");

		events->ShutDown(pEncoder);
		EventAdvidsed = FALSE;

		pEncoder->Release();
		audio = FALSE;
		video = FALSE;
		bEncoder = FALSE;
	}
	Fire_BroadcastStatus(L"Session Closed");
	bSession = false;
	return S_OK;
}

STDMETHODIMP CBroadcastIt::Status()
{
	if(!bSession)
		return S_FALSE;

	HRESULT hr;
    IWMEncStatistics* pStatistics;
    IWMEncOutputStats* pOutputStats;
    IDispatch* pDispOutputStats;

    short iStreamCount;
    long lAvgBitrate, lAvgSampleRate;  long lCurrentBitRate, lCurrentSampleRate;   long lExpectedBitRate, lExpectedSampleRate;    CURRENCY  qwByteCount, qwSampleCount;   CURRENCY  qwDroppedByteCount, qwDroppedSampleCount;
	
	// Initialize the COM library and retrieve a pointer
	// to an IWMEncoder interface.

    hr = CoInitialize(NULL);
    CoCreateInstance(CLSID_WMEncoder,
                     NULL,
                     CLSCTX_INPROC_SERVER,
                     IID_IWMEncoder,
                    (void**) &pEncoder);

	// Retrieve an IWMEncStatistics interface pointer.

    hr = pEncoder->get_Statistics(&pStatistics);

	// Retrieve the number of multiple bit rate output streams.

    hr = pStatistics->get_StreamOutputCount(WMENC_VIDEO,
                                            0,
                                            &iStreamCount);

// Retrieve an IDispatch pointer for the IWMEncOutputStats
// interface.

    hr = pStatistics->get_StreamOutputStats(WMENC_VIDEO,
                                            0,
                                            0,
                                            &pDispOutputStats);

// Call QueryInterface for the IWMEncNetConnectionStats
// interface pointer.

    hr = pDispOutputStats->QueryInterface(IID_IWMEncOutputStats, 
                                          (void**)&pOutputStats);

// Manually configure the encoder engine or load
// a configuration from a file. For an example, see the 
// IWMEncFile object.

// You can create a timer to retrieve the statistics
// after you start the encoder engine.

    hr = pOutputStats->get_AverageBitrate(&lAvgBitrate);
    hr = pOutputStats->get_AverageSampleRate(&lAvgSampleRate);
    hr = pOutputStats->get_ByteCount(&qwByteCount);
    hr = pOutputStats->get_CurrentBitrate(&lCurrentBitRate);
    hr = pOutputStats->get_CurrentSampleRate(&lCurrentSampleRate);
    hr = pOutputStats->get_DroppedByteCount(&qwDroppedByteCount);
    hr = pOutputStats->get_DroppedSampleCount(&qwDroppedSampleCount);
    hr = pOutputStats->get_ExpectedBitrate(&lExpectedBitRate);
    hr = pOutputStats->get_ExpectedSampleRate(&lExpectedSampleRate);
    hr = pOutputStats->get_SampleCount(&qwSampleCount);


	CURRENCY  TimeElpsed;
	pStatistics->get_EncodingTime(&TimeElpsed);
	Fire_EncoderStatus(lAvgBitrate,lAvgSampleRate,lCurrentBitRate,
							lCurrentSampleRate, lExpectedBitRate,
							lExpectedSampleRate,qwByteCount,qwDroppedByteCount,
							qwDroppedSampleCount,qwSampleCount
							,TimeElpsed);

	return S_OK;
}



STDMETHODIMP CBroadcastIt::SendScript(BSTR bstrScript)
{
	if(!bSession)
		return S_FALSE;
	CComBSTR bstrType(L"TEXT");
    CComBSTR bstrData(bstrScript);
    pEncoder->SendScript(0, bstrType, bstrData);
	return S_OK;
}

STDMETHODIMP CBroadcastIt::SendURL(BSTR bstrURL)
{
	if(!bSession)
		return S_FALSE;
	CComBSTR bstrType(L"URL");
    CComBSTR bstrData(bstrURL);
    pEncoder->SendScript(0, bstrType, bstrData);
	return S_OK;
}


STDMETHODIMP CBroadcastIt::PrepareToEncode()
{
	if(!bEncoder)
		return S_FALSE;

	if(!bSession)
		return S_FALSE;

	USES_CONVERSION;
	HRESULT hr;
	
	
	/************ Broadcast ***************/

	IWMEncBroadcast* pBrdcst;
	long PortNum;

	hr = pEncoder->get_Broadcast(&pBrdcst);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"problem calling get_Broadcast");
		return S_FALSE;
	}

	hr = pBrdcst->get_PortNumber(WMENC_PROTOCOL_HTTP, &PortNum);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"problem calling get_PortNumber");
		return S_FALSE;
	}

	PortNum = shPortNo;

	hr = pBrdcst->put_PortNumber(WMENC_PROTOCOL_HTTP, PortNum);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"problem calling put_PortNumber");
		return S_FALSE;
	}

	Fire_BroadcastStatus(bstrProfile);

	char no1[10];
	itoa(shPortNo,no1,10);
	Fire_BroadcastStatus(T2OLE(_T(no1)));

	
	/************ Profile Setting ***************/

	IWMEncProfileCollection* pProColl;
	IWMEncProfile* pPro;

	long lCount;
	BOOL found = FALSE;

	CComBSTR bstrName(L"");

	hr = pEncoder->get_ProfileCollection(&pProColl);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Get Profile Collaction Failed");
		return S_FALSE;
	}

	CComVariant varProfile;
	varProfile.vt = VT_DISPATCH;
	CComBSTR tempProf;

	hr = pProColl->get_Count(&lCount);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Get Profile Count Faile");
		return S_FALSE;
	}

	if(audio && video)
		//tempProf.Attach(T2OLE("Encoding Profile for P2P Streaming Demo"));
		tempProf.Attach(T2OLE("Video for Web servers (28.8 kbps)"));
	else if(audio && !video)
		//tempProf.Attach(T2OLE("Encoding Audio Profile for P2P Streaming Demo"));
		tempProf.Attach(T2OLE("Video for Web servers (28.8 kbps)"));
	else
	{
		HeraldThisMessage(L"Set Profile Failed");
		return S_FALSE;
	}

    for (int i=0; i<lCount; i++)
    {
        hr = pProColl->Item(i, &pPro);
        hr = pPro->get_Name(&bstrName);
        if (_wcsicmp(bstrName, tempProf)==0)
        {
			varProfile.pdispVal = pPro;
			hr = pSrcGrp->put_Profile(varProfile);
			found = TRUE;
	        break;
        }
    }

	if(!found)
	{
		HeraldThisMessage(L"Setting Profile Failed");
		return S_FALSE;
	}

	hr = pEncoder->PrepareToEncode(VARIANT_TRUE);
	if(FAILED(hr))
	{
		if(audio && !video)
		{
			Fire_BroadcastStatus(L"problem calling PrepareToEncode");
			return S_FALSE;
		}

		video = FALSE;
		CComVariant varIndex;
		varIndex.vt = VT_I2;
		varIndex.iVal = 0;
		hr = pSrcGrp->RemoveSource(WMENC_VIDEO, varIndex);
		tempProf.Attach(T2OLE("Encoding Audio Profile for P2P Streaming Demo"));

		for (int i=0; i<lCount; i++)
		{
			hr = pProColl->Item(i, &pPro);
			hr = pPro->get_Name(&bstrName);
			if (_wcsicmp(bstrName, tempProf)==0)
			{
				varProfile.pdispVal = pPro;
				hr = pSrcGrp->put_Profile(varProfile);
				found = TRUE;
				break;
			}
		}

		if(!found)
		{
			HeraldThisMessage(L"Profile Not Found");
			return S_FALSE;
		}

		hr = pEncoder->PrepareToEncode(VARIANT_TRUE);
	
		if(FAILED(hr))
		{
			Fire_BroadcastStatus(L"Second attempt of calling PrepareToEncode failed");
			return S_FALSE;
		}
	}
	return S_OK;
}



STDMETHODIMP CBroadcastIt::InitializeBroadcaster()
{
	HRESULT hr = CoCreateInstance(CLSID_WMEncoder,
					NULL,
					CLSCTX_INPROC_SERVER,
					IID_IWMEncoder,
				   (void**) &pEncoder);

	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Encoder Couldn't be Initialized");
		return S_FALSE;
	}

	hr = pEncoder->get_SourceGroupCollection(&pSrcGrpColl);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Source group collection retrieval failed");
		return S_FALSE;
	}

	hr = pSrcGrpColl->Add(L"SG_2", &pSrcGrp);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Sorce group couldn't be added");
		return S_FALSE;
	}

	bEncoder = TRUE;
	Fire_BroadcastStatus(L"Encoder Initailized");
	return S_OK;
}




/******************** Properties *********************/

/****************** Put Properties *******************/

STDMETHODIMP CBroadcastIt::put_Profile(BSTR newVal)
{
	USES_CONVERSION;
	bstrProfile = newVal;
	char no[10];
	HeraldThisMessage(T2OLE(_T(no)));

	return S_OK;
}

STDMETHODIMP CBroadcastIt::put_Port(short newVal)
{
	USES_CONVERSION;
	shPortNo = newVal;
	char no[10];
	itoa(shPortNo,no,10);
	HeraldThisMessage(T2OLE(_T(no)));
	return S_OK;
}

STDMETHODIMP CBroadcastIt::put_AudioMedia(BSTR newVal)
{
	bstrAudioMedia = newVal;
	HeraldThisMessage(bstrAudioMedia);

	IWMEncSource* pAudSrc; 
	HRESULT hr = pSrcGrp->AddSource(WMENC_AUDIO, &pAudSrc);
	Fire_BroadcastStatus(L"AddSource");
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Specifing audio Source Failed");
		audio = FALSE;
		return S_FALSE;
	}
	hr = pAudSrc->SetInput(bstrAudioMedia.Copy());
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Setting audio Media Source Failed");
		CComVariant varIndex;
		varIndex.vt = VT_I2;
		varIndex.iVal = 0;
		hr = pSrcGrp->RemoveSource(WMENC_AUDIO, varIndex);
		audio = FALSE;
		return S_FALSE;
	}

	audio = TRUE;
	return S_OK;
}

STDMETHODIMP CBroadcastIt::put_VideoMedia(BSTR newVal)
{
	bstrVideoMedia = newVal;
	HeraldThisMessage(bstrVideoMedia);
	IWMEncSource* pVidSrc;

	HRESULT hr = pSrcGrp->AddSource(WMENC_VIDEO, &pVidSrc);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Specifing video Source Failed");
		video = FALSE;
		return S_FALSE;
	}
	hr = pVidSrc->SetInput(bstrVideoMedia.Copy());
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Setting Video Media Source Failed");
		video = FALSE;
		CComVariant varIndex;
		varIndex.vt = VT_I2;
		varIndex.iVal = 0;
		hr = pSrcGrp->RemoveSource(WMENC_VIDEO, varIndex);
		return S_FALSE;
	}
	Fire_BroadcastStatus(bstrVideoMedia.Copy());

	video = TRUE;
	return S_OK;
}

STDMETHODIMP CBroadcastIt::put_UseScript(short newVal)
{
	if((newVal == 0) || newVal == 1)
	{
		shUseScript = newVal;
	}
	else
		Fire_BroadcastStatus(L"Use Script = 1 or Dont use Script = 0");
	return S_OK;
}


/****************** Get Properties *******************/

STDMETHODIMP CBroadcastIt::get_Profile(BSTR *pVal)		{	*pVal = bstrProfile.Copy();		return S_OK;}
STDMETHODIMP CBroadcastIt::get_Port(short *pVal)		{	*pVal = shPortNo;				return S_OK;}
STDMETHODIMP CBroadcastIt::get_AudioMedia(BSTR *pVal)	{	*pVal = bstrAudioMedia.Copy(); 	return S_OK;}
STDMETHODIMP CBroadcastIt::get_VideoMedia(BSTR *pVal)	{	*pVal = bstrVideoMedia.Copy();	return S_OK;}
STDMETHODIMP CBroadcastIt::get_UseScript(short *pVal)	{	*pVal = shUseScript;			return S_OK;}


STDMETHODIMP CBroadcastIt::get_AudioDevices(VARIANT *pVal)
{
	HRESULT hr;

    IWMEncSourcePluginInfoManager* pSrcPlugMgr;
    IWMEncPluginInfo* pPlugInfo;

    int j,i;
    long lPlugCount, lResCount;
    VARIANT_BOOL bResources;


    hr = pEncoder->get_SourcePluginInfoManager(&pSrcPlugMgr);
    hr = pSrcPlugMgr->get_Count(&lPlugCount);

    for (i=0; i<lPlugCount; i++)
    {
        hr = pSrcPlugMgr->Item(i, &pPlugInfo);
        CComBSTR bstrScheme;
        hr = pPlugInfo->get_SchemeType(&bstrScheme);

        if (_wcsicmp(bstrScheme, L"DEVICE")==0 || _wcsicmp(bstrScheme, L"UserScript")==0)
        {
            hr = pPlugInfo->get_Resources(&bResources); 
            if (bResources==VARIANT_TRUE)
            {
                hr = pPlugInfo->get_Count(&lResCount);


				VariantInit(pVal);
				pVal->vt = VT_ARRAY | VT_BSTR;
				SAFEARRAY *pTheArray;
				SAFEARRAYBOUND pBounds = {lResCount,0};
				pTheArray = SafeArrayCreate(VT_BSTR,1,&pBounds);
				BSTR *bstrArray;
				SafeArrayAccessData(pTheArray,reinterpret_cast<void**>(&bstrArray));


                for (j=0; j<lResCount; j++)
                {
                    CComBSTR bstrResource;
                    hr = pPlugInfo->Item(j, &bstrResource);
					bstrArray[j] = bstrResource.Copy();
					//MessageBox(OLE2T(bstrResource),"adf",MB_OK);
                }

				SafeArrayUnaccessData(pTheArray);
				pVal->parray = pTheArray;
				break;
            }
        }
    }
	return S_OK;
}


/****  Get Available Profiles For This Media Types ****/

STDMETHODIMP CBroadcastIt::get_ProfilesList(VARIANT *pVal)
{
	if(!bEncoder)
		return S_FALSE;

	HRESULT hr;
	IWMEncProfileCollection* pProColl;
	IWMEncProfile* pPro;

	hr = pEncoder->get_ProfileCollection(&pProColl);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Retrieving Collection of Profiles failed");
		return S_FALSE;
	}


	long lCount;
	hr = pProColl->get_Count(&lCount);
	if(FAILED(hr))
	{
		Fire_BroadcastStatus(L"Retrieving Count of Profiles failed");
		return S_FALSE;
	}

	VariantInit(pVal);
	pVal->vt = VT_ARRAY | VT_BSTR;
	SAFEARRAY *pTheArray;
	SAFEARRAYBOUND pBounds = {lCount,0};
	pTheArray = SafeArrayCreate(VT_BSTR,1,&pBounds);
	BSTR *bstrArray;
	SafeArrayAccessData(pTheArray,reinterpret_cast<void**>(&bstrArray));

//	char ch[100];
	USES_CONVERSION;
	for (int i=0; i<lCount; i++)
	{
		CComBSTR m_bstrName;
		hr = pProColl->Item(i, &pPro);
		hr = pPro->get_Name(&m_bstrName);
		bstrArray[i] = m_bstrName.Copy();
		/*wsprintf(ch,"%d:   %s",i,OLE2T(m_bstrName));
		MessageBox(ch,"Name",MB_OK);*/
	}
	
	SafeArrayUnaccessData(pTheArray);
	pVal->parray = pTheArray;
	return S_OK;
}