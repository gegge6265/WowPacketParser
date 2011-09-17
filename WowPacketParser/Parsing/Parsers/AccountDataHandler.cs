using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;

namespace WowPacketParser.Parsing.Parsers
{
    public static class AccountDataHandler
    {
        [Parser(Opcode.SMSG_ACCOUNT_DATA_TIMES)]
        public static void HandleAccountDataTimes(Packet packet)
        {
            packet.ReadTime("Unk Time");

            packet.ReadByte("Unk Byte");

            var mask = packet.ReadInt32("Time Mask");

            for (var i = 0; i < 8; i++)
            {
                if ((mask & (1 << i)) == 0)
                    continue;

                var unkTime2 = packet.ReadInt32();
                Console.WriteLine("Unk Int32 2 " + i + ": " + unkTime2);
            }
        }

        [Parser(Opcode.CMSG_REQUEST_ACCOUNT_DATA)]
        public static void HandleRequestAccountData(Packet packet)
        {
            packet.ReadEnum<AccountDataType>("Data Type", TypeCode.Int32);
        }

        public static void ReadUpdateAccountDataBlock(Packet packet)
        {
            packet.ReadEnum<AccountDataType>("Data Type", TypeCode.Int32);

            packet.ReadTime("Unk Time");

            var decompCount = packet.ReadInt32();
            packet = packet.Inflate(decompCount);

            var data = packet.ReadBytes(decompCount);
            Console.Write("Account Data: ");

            foreach (var b in data)
                Console.Write((char)b);

            Console.WriteLine();
        }

        [Parser(Opcode.CMSG_UPDATE_ACCOUNT_DATA)]
        public static void HandleClientUpdateAccountData(Packet packet)
        {
            ReadUpdateAccountDataBlock(packet);
        }

        [Parser(Opcode.SMSG_UPDATE_ACCOUNT_DATA)]
        public static void HandleServerUpdateAccountData(Packet packet)
        {
            packet.ReadGuid("GUID");
            ReadUpdateAccountDataBlock(packet);
        }

        [Parser(Opcode.SMSG_UPDATE_ACCOUNT_DATA_COMPLETE)]
        public static void HandleUpdateAccountDataComplete(Packet packet)
        {
            packet.ReadEnum<AccountDataType>("Data Type", TypeCode.Int32);
            packet.ReadInt32("Unk Int32");
        }
    }
}
