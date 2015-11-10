namespace VCSJones.FiddlerCertGen
open System.Runtime.InteropServices
open ExternInterop

module internal NCryptProperties =
    let Read (convert : (nativeint -> 'a)) (handle : NCryptHandleBase) (propertyName : string)  =
        let mutable size = 0u
        if NCryptGetProperty(handle, propertyName, 0n, 0u, &size, 0u) <> ERROR_SUCCESS then
            invalidOp("Unable to query property.")
        use buffer = GlobalBufferSafeHandle.Allocate(int size)
        if NCryptGetProperty(handle, propertyName, buffer.DangerousGetHandle(), size, &size, 0u) <> ERROR_SUCCESS then
            invalidOp("Unable to query property.")
        convert(buffer.DangerousGetHandle())

    let ReadStringUnicode : NCryptHandleBase -> string -> string = Read Marshal.PtrToStringUni
    let ReadInt32 : NCryptHandleBase -> string -> int  = Read Marshal.ReadInt32

    let WriteStringUnicode (handle : NCryptHandleBase) (propertyName : string) (value : string) =
        let ptr = Marshal.StringToHGlobalUni(value)
        use valueHandle = GlobalBufferSafeHandle.FromHandle(ptr)
        let result = NCryptSetProperty(handle, propertyName, valueHandle.DangerousGetHandle(), uint32(value.Length*2), 0u)
        if result <> ERROR_SUCCESS then invalidOp("Unable to set property")

    let WriteInt32 (handle : NCryptHandleBase) (propertyName : string) (value : int32) =
        use valueHandle = GlobalBufferSafeHandle.Allocate(sizeof<int>)
        Marshal.WriteInt32(valueHandle.DangerousGetHandle(), value)
        let result = NCryptSetProperty(handle, propertyName, valueHandle.DangerousGetHandle(), uint32(sizeof<int>), 0u)
        if result <> ERROR_SUCCESS then invalidOp("Unable to set property")

    let WriteUInt32 (handle : NCryptHandleBase) (propertyName : string) (value : uint32) = WriteInt32 handle propertyName (int32(value))
    
    let WriteEnum<'T, 'A when 'T : enum<'A>>(handle : NCryptHandleBase) (propertyName : string) (value : 'T) =
        match typeof<'A> with
        | t when t = typeof<int32> -> WriteInt32 handle propertyName (value :> obj :?> int)
        | t when t = typeof<uint32> -> WriteUInt32 handle propertyName (value :> obj :?> uint32)
        | _ -> invalidArg "value " "Unsupported enum type."