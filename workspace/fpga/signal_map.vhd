library ieee;
use ieee.std_logic_1164.all;

entity signal_map is
  port (
    clk : in std_logic;
    channel_active : in std_logic_vector(7 downto 0);
    timing_valid : out std_logic
  );
end entity signal_map;

architecture rtl of signal_map is
begin
  timing_valid <= '1' when channel_active /= "00000000" else '0';
end architecture rtl;
