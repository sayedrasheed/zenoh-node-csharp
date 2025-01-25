#pragma warning disable CS8500

using System.Runtime.InteropServices;

namespace Zenoh;

// z_congestion_control_t
public enum CongestionControl
{
    Block,
    Drop
}

// z_encoding_prefix_t
public enum EncodingPrefix
{
    Empty = 0,
    AppOctetStream = 1,
    AppCustom = 2,
    TextPlain = 3,
    AppProperties = 4,
    AppJson = 5,
    AppSql = 6,
    AppInteger = 7,
    AppFloat = 8,
    AppXml = 9,
    AppXhtmlXml = 10,
    AppXWwwFormUrlencoded = 11,
    TextJson = 12,
    TextHtml = 13,
    TextXml = 14,
    TextCss = 15,
    TextCsv = 16,
    TextJavascript = 17,
    ImageJpeg = 18,
    ImagePng = 19,
    ImageGif = 20
}

// z_sample_kind_t
public enum SampleKind
{
    Put = 0,
    Delete = 1
}

// z_priority_t
public enum Priority
{
    RealTime = 1,
    InteractiveHigh = 2,
    InteractiveLow = 3,
    DataHigh = 4,
    Data = 5,
    DataLow = 6,
    Background = 7
}

// z_reliability_t
public enum Reliability
{
    BestEffort,
    Reliable
}

// z_bytes_t
// --------------------------------
//  typedef struct z_bytes_t {
//      size_t len;
//      const uint8_t *start;
//  } z_bytes_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal unsafe struct ZBytes
{
    internal nuint len;
    internal byte* start;
}

// z_id_t
// --------------------------------
//  typedef struct z_id_t {
//      uint8_t id[16];
//  } z_id_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal unsafe struct ZId
{
    // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    internal fixed byte id[16];
}

// z_encoding_t
// --------------------------------
//  typedef struct z_encoding_t {
//      enum z_encoding_prefix_t prefix;
//      struct z_bytes_t suffix;
//  } z_encoding_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZEncoding
{
    internal EncodingPrefix prefix;
    internal ZBytes suffix;
}

// z_timestamp_t
// --------------------------------
//  typedef struct z_timestamp_t {
//      uint64_t time;
//      struct z_id_t id;
//  } z_timestamp_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZTimestamp
{
    internal ulong time;
    internal ZId id;
}

// z_keyexpr_t  
// --------------------------------
//  typedef struct z_keyexpr_t {
//      uint64_t _0[4];
//  } z_keyexpr_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal unsafe struct ZKeyexpr
{
    // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    private fixed ulong _[4];
}

// z_sample_t
// --------------------------------
//  typedef struct z_sample_t {
//      struct z_keyexpr_t keyexpr;
//      struct z_bytes_t payload;
//      struct z_encoding_t encoding;
//      const void *_zc_buf;
//      enum z_sample_kind_t kind;
//      struct z_timestamp_t timestamp;
//  } z_sample_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZSample
{
    internal ZKeyexpr keyexpr;
    internal ZBytes payload;
    internal ZEncoding encoding;
    private nint _zc_buf;
    internal SampleKind kind;
    internal ZTimestamp timestamp;
}

// z_owned_config_t
// --------------------------------
//  typedef struct z_owned_config_t {
//      void *_0;
//  } z_owned_config_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZOwnedConfig
{
    private nint _;
}

// z_config_t
// --------------------------------
//  typedef struct z_config_t {
//      const struct z_owned_config_t *_0;
//  } z_config_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZConfig
{
    private nint _;
}

// z_subscriber_options_t 
// --------------------------------
// typedef struct z_subscriber_options_t {
//   enum z_reliability_t reliability;
// } z_subscriber_options_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZSubscriberOptions
{
    internal Reliability reliability;
}

// z_owned_subscriber_t 
// --------------------------------
//  typedef struct ALIGN(8) z_owned_subscriber_t {
//      uint64_t _0[1];
//  } z_owned_subscriber_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ZOwnedSubscriber
{
    private ulong _;
}


// --------------------------------
//  typedef struct ALIGN(8) z_owned_reply_t {
//      uint64_t _0[22];
//  } z_owned_reply_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal unsafe struct ZOwnedReply
{
    private fixed ulong _[22];
}

// z_value_t 
// --------------------------------
//  typedef struct z_value_t {
//      struct z_bytes_t payload;
//      struct z_encoding_t encoding;
//  } z_value_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZValue
{
    internal ZBytes payload;
    internal ZEncoding encoding;
}

// z_put_options_t 
// --------------------------------
//  typedef struct z_put_options_t {
//      struct z_encoding_t encoding;
//      enum z_congestion_control_t congestion_control;
//      enum z_priority_t priority;
//  } z_put_options_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZPutOptions
{
    internal ZEncoding encoding;
    internal CongestionControl congestionControl;
    internal Priority priority;
}

// z_query_t 
// --------------------------------
// typedef struct z_query_t {
//   void *_0;
// } z_query_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZQuery
{
    private nint _;
}

// z_owned_session_t 
// --------------------------------
//  typedef struct z_owned_session_t {
//    uintptr_t _0;
//  } z_owned_session_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZOwnedSession
{
    private nint _;
}

// z_session_t 
// --------------------------------
// typedef struct z_session_t {
//     uintptr_t _0;
// } z_session_t;
// --------------------------------
[StructLayout(LayoutKind.Sequential)]
internal struct ZSession
{
    private nint _;
}

// z_owned_closure_sample_t 
// --------------------------------
//  typedef struct z_owned_closure_sample_t {
//      void *context;
//      void (*call)(const struct z_sample_t*, void *context);
//      void (*drop)(void*);
//  } z_owned_closure_sample_t;
// --------------------------------
internal unsafe delegate void ZOwnedClosureSampleCall(ZSample* sample, void* context);

internal unsafe delegate void ZOwnedClosureSampleDrop(void* context);

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct ZOwnedClosureSample
{
    internal void* context;
    internal ZOwnedClosureSampleCall call;
    internal ZOwnedClosureSampleDrop? drop;
}

internal static unsafe class ZenohC
{
    internal const string DllName = "zenohc";
    internal static string zConfigMulticastIpv4AddressKey = "scouting/multicast/address";

    [DllImport(DllName, EntryPoint = "z_config_drop", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void z_config_drop(ZOwnedConfig* config);

    [DllImport(DllName, EntryPoint = "z_config_loan", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ZConfig z_config_loan(ZOwnedConfig* config);

    [DllImport(DllName, EntryPoint = "z_config_default", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ZOwnedConfig z_config_default();

    [DllImport(DllName, EntryPoint = "zc_config_insert_json", CallingConvention = CallingConvention.Cdecl)]
    internal static extern sbyte zc_config_insert_json(
        ZConfig config, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport(DllName, EntryPoint = "z_keyexpr", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ZKeyexpr z_keyexpr(byte* name);

    [DllImport(DllName, EntryPoint = "z_declare_subscriber", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ZOwnedSubscriber z_declare_subscriber(
        ZSession session, ZKeyexpr keyexpr, ZOwnedClosureSample* callback, ZSubscriberOptions* options);

    [DllImport(DllName, EntryPoint = "z_put", CallingConvention = CallingConvention.Cdecl)]
    internal static extern sbyte z_put(ZSession session, ZKeyexpr keyexpr, byte* payload, nuint len, ZPutOptions* opts);

    [DllImport(DllName, EntryPoint = "z_put_options_default", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ZPutOptions z_put_options_default();

    [DllImport(DllName, EntryPoint = "z_open", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ZOwnedSession z_open(ZOwnedConfig* config);

    [DllImport(DllName, EntryPoint = "z_close", CallingConvention = CallingConvention.Cdecl)]
    internal static extern sbyte z_close(ZOwnedSession* session);

    [DllImport(DllName, EntryPoint = "z_session_check", CallingConvention = CallingConvention.Cdecl)]
    internal static extern sbyte z_session_check(ZOwnedSession* session);

    [DllImport(DllName, EntryPoint = "z_session_loan", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ZSession z_session_loan(ZOwnedSession* session);
}