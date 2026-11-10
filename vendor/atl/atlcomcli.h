// Minimal ATL stub for CI builds (no system ATL required)
// Provides only the symbols commonly used by Rectify11CPL
#pragma once

#include <windows.h>
#include <ole2.h>
#include <olectl.h>

#ifndef ATLASSERT
#define ATLASSERT(x) ((void)0)
#endif

namespace ATL
{

template <class T>
class CComPtr
{
public:
    T* p;

    CComPtr() : p(nullptr) {}
    CComPtr(T* lp) : p(lp) { if (p) p->AddRef(); }
    CComPtr(const CComPtr& other) : p(other.p) { if (p) p->AddRef(); }
    ~CComPtr() { if (p) p->Release(); }

    CComPtr& operator=(T* lp)
    {
        if (p) p->Release();
        p = lp;
        if (p) p->AddRef();
        return *this;
    }

    CComPtr& operator=(const CComPtr& other)
    {
        if (this != &other)
        {
            if (p) p->Release();
            p = other.p;
            if (p) p->AddRef();
        }
        return *this;
    }

    operator T*() const { return p; }
    T** operator&() { return &p; }
    T* operator->() const { return p; }

    void Release()
    {
        if (p) { p->Release(); p = nullptr; }
    }

    void Attach(T* lp)
    {
        if (p) p->Release();
        p = lp;
    }

    T* Detach()
    {
        T* tmp = p;
        p = nullptr;
        return tmp;
    }

    HRESULT CoCreateInstance(REFCLSID rclsid, REFIID riid, LPUNKNOWN pOuter = nullptr, DWORD ClsCtx = CLSCTX_ALL)
    {
        return ::CoCreateInstance(rclsid, pOuter, ClsCtx, riid, (void**)&p);
    }
};

class CComBSTR
{
public:
    BSTR m_str;
    CComBSTR() : m_str(nullptr) {}
    CComBSTR(LPCWSTR src) : m_str(src ? ::SysAllocString(src) : nullptr) {}
    ~CComBSTR() { if (m_str) ::SysFreeString(m_str); }
    operator BSTR() const { return m_str; }
    BSTR* operator&() { return &m_str; }
};

class CComVariant : public VARIANT
{
public:
    CComVariant() { ::VariantInit(this); }
    ~CComVariant() { ::VariantClear(this); }
    HRESULT Clear() { return ::VariantClear(this); }
};

} // namespace ATL

using namespace ATL;
