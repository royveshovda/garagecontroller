# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: GarageControllerMessages.proto

from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import descriptor_pb2
# @@protoc_insertion_point(imports)




DESCRIPTOR = _descriptor.FileDescriptor(
  name='GarageControllerMessages.proto',
  package='',
  serialized_pb='\n\x1eGarageControllerMessages.proto\"t\n\x11ToggleDoorCommand\x12\x11\n\tSessionId\x18\x01 \x01(\t\x12\x15\n\nDoorNumber\x18\x02 \x01(\x05:\x01\x30\x12\x12\n\x07\x43reated\x18\x03 \x01(\x03:\x01\x30\x12\x0e\n\x06\x45xpiry\x18\x04 \x01(\x03\x12\x11\n\tSignature\x18\x05 \x01(\t')




_TOGGLEDOORCOMMAND = _descriptor.Descriptor(
  name='ToggleDoorCommand',
  full_name='ToggleDoorCommand',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='SessionId', full_name='ToggleDoorCommand.SessionId', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=str(b"", "utf-8"),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='DoorNumber', full_name='ToggleDoorCommand.DoorNumber', index=1,
      number=2, type=5, cpp_type=1, label=1,
      has_default_value=True, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='Created', full_name='ToggleDoorCommand.Created', index=2,
      number=3, type=3, cpp_type=2, label=1,
      has_default_value=True, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='Expiry', full_name='ToggleDoorCommand.Expiry', index=3,
      number=4, type=3, cpp_type=2, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    _descriptor.FieldDescriptor(
      name='Signature', full_name='ToggleDoorCommand.Signature', index=4,
      number=5, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=str(b"", "utf-8"),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  options=None,
  is_extendable=False,
  extension_ranges=[],
  serialized_start=34,
  serialized_end=150,
)

DESCRIPTOR.message_types_by_name['ToggleDoorCommand'] = _TOGGLEDOORCOMMAND

class ToggleDoorCommand(_message.Message, metaclass=_reflection.GeneratedProtocolMessageType):
  DESCRIPTOR = _TOGGLEDOORCOMMAND

  # @@protoc_insertion_point(class_scope:ToggleDoorCommand)


# @@protoc_insertion_point(module_scope)
