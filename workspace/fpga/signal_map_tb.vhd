library ieee;
use ieee.std_logic_1164.all;

entity signal_map_tb is
end entity signal_map_tb;

architecture test of signal_map_tb is
  signal clk : std_logic := '0';
  signal channel_active : std_logic_vector(7 downto 0) := "00000001";
  signal timing_valid : std_logic;
begin
  uut: entity work.signal_map port map(clk => clk, channel_active => channel_active, timing_valid => timing_valid);
  clk <= not clk after 5 ns;
end architecture test;
