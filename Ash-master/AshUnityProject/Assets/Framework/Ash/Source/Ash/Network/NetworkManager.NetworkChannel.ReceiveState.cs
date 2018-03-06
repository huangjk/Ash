﻿






using System.IO;

namespace Ash.Core.Network
{
    internal partial class NetworkManager
    {
        private partial class NetworkChannel
        {
            private sealed class ReceiveState
            {
                private const int DefaultBufferLength = 1024 * 8;
                private readonly MemoryStream m_Stream;
                private IPacketHeader m_PacketHeader;

                public ReceiveState()
                {
                    m_Stream = new MemoryStream(DefaultBufferLength);
                    m_PacketHeader = null;
                }

                public MemoryStream Stream
                {
                    get
                    {
                        return m_Stream;
                    }
                }

                public IPacketHeader PacketHeader
                {
                    get
                    {
                        return m_PacketHeader;
                    }
                }

                public void PrepareForPacketHeader(int packetHeaderLength)
                {
                    Reset(packetHeaderLength, null);
                }

                public void PrepareForPacket(IPacketHeader packetHeader)
                {
                    if (packetHeader == null)
                    {
                        throw new AshException("Packet header is invalid.");
                    }

                    Reset(packetHeader.PacketLength, packetHeader);
                }

                private void Reset(int targetLength, IPacketHeader packetHeader)
                {
                    if (targetLength < 0)
                    {
                        throw new AshException("Target length is invalid.");
                    }

                    m_Stream.Position = 0L;
                    m_Stream.SetLength(targetLength);
                    m_PacketHeader = packetHeader;
                }
            }
        }
    }
}
