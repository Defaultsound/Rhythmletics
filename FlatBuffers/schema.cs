// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace NetworkPacket
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Vec3 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public Vec3 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float X { get { return __p.bb.GetFloat(__p.bb_pos + 0); } }
  public float Y { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }
  public float Z { get { return __p.bb.GetFloat(__p.bb_pos + 8); } }

  public static Offset<NetworkPacket.Vec3> CreateVec3(FlatBufferBuilder builder, float X, float Y, float Z) {
    builder.Prep(4, 12);
    builder.PutFloat(Z);
    builder.PutFloat(Y);
    builder.PutFloat(X);
    return new Offset<NetworkPacket.Vec3>(builder.Offset);
  }
};

public struct PlayerInformation : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static PlayerInformation GetRootAsPlayerInformation(ByteBuffer _bb) { return GetRootAsPlayerInformation(_bb, new PlayerInformation()); }
  public static PlayerInformation GetRootAsPlayerInformation(ByteBuffer _bb, PlayerInformation obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public PlayerInformation __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string ID { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetIDBytes() { return __p.__vector_as_span<byte>(4, 1); }
#else
  public ArraySegment<byte>? GetIDBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public byte[] GetIDArray() { return __p.__vector_as_array<byte>(4); }
  public NetworkPacket.Vec3? Position { get { int o = __p.__offset(6); return o != 0 ? (NetworkPacket.Vec3?)(new NetworkPacket.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public NetworkPacket.Vec3? Rotation { get { int o = __p.__offset(8); return o != 0 ? (NetworkPacket.Vec3?)(new NetworkPacket.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartPlayerInformation(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddID(FlatBufferBuilder builder, StringOffset IDOffset) { builder.AddOffset(0, IDOffset.Value, 0); }
  public static void AddPosition(FlatBufferBuilder builder, Offset<NetworkPacket.Vec3> PositionOffset) { builder.AddStruct(1, PositionOffset.Value, 0); }
  public static void AddRotation(FlatBufferBuilder builder, Offset<NetworkPacket.Vec3> RotationOffset) { builder.AddStruct(2, RotationOffset.Value, 0); }
  public static Offset<NetworkPacket.PlayerInformation> EndPlayerInformation(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<NetworkPacket.PlayerInformation>(o);
  }
};


}
