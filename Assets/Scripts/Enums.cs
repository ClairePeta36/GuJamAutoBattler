using System;
using System.Linq;

    public enum Keyword : sbyte
    {
        None            = -1,
        Afterlife       =  0, 
        Frontline       =  1,
        TwinStrike      =  2,
        Hidden          =  3,
        Protected       =  4,
        Leech           =  5,
        Roar            =  6,
        Deadly          =  7,
        Blitz           =  8,
        Flank           =  9,
        Confused        = 10,
        Backline        = 11,
        Regen           = 12,
        Ward            = 13, 
        Armor           = 14,
    }

    public enum Tribe : UInt16
    {
        None = 0,               // 0000 0000 0000 0000
                  
        Viking = 1,               // 0000 0000 0000 0001
        Amazon = 1 <<  1,         // 0000 0000 0000 0010
        Aether = 1 <<  2,         // 0000 0000 0000 0100
        Atlantean = 1 <<  3,         // 0000 0000 0000 1000
        Olympian = 1 <<  4,         // 0000 0000 0001 0000
        Nether = 1 <<  5,         // 0000 0000 0010 0000
        Anubian = 1 <<  6,         // 0000 0000 0100 0000
        Dragon = 1 <<  7,         // 0000 0000 1000 0000
        Wild = 1 <<  8,         // 0000 0001 0000 0000
        Mystic = 1 <<  9,         // 0000 0010 0000 0000
        Structure = 1 << 10,         // 0000 0100 0000 0000
        Guild = 1 << 11,         // 0000 1000 0000 0000
        Blessing = 1 << 12,         // 0001 0000 0000 0000
                
        All = UInt16.MaxValue, // 1111 1111 1111 1111
    };