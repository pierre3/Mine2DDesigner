﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MinecraftBlockBuilder.Models
{
    internal record Block
    {
        public string Name { get; init; }
        public Textures Textures { get; init; }
        public Block(string name)
        {
            Name = name;
            Textures = name == "air" ? new Textures() : new Textures(name);
        }

        public static IReadOnlyList<Block> Definitions { get; } = new ReadOnlyCollection<Block>(new[]
        {
            new Block( "air"),
            new Block("stone"),
            new Block("granite"),
            new Block("polished_granite"),
            new Block("diorite"),
            new Block("polished_diorite"),
            new Block("andesite"),
            new Block("polished_andesite"),
            new Block("deepslate"),
            new Block("polished_deepslate"),
            new Block("calcite"),
            new Block("tuff"),
            new Block("grass_block"),
            new Block("dirt"),
            new Block("coarse_dirt"),
            new Block("podzol"),
            new Block("rooted_dirt"),
            new Block("mud"),
            new Block("crimson_nylium"),
            new Block("warped_nylium"),
            new Block("cobblestone"),
            new Block("oak_planks"),
            new Block("spruce_planks"),
            new Block("birch_planks"),
            new Block("jungle_planks"),
            new Block("acacia_planks"),
            new Block("dark_oak_planks"),
            new Block("mangrove_planks"),
            new Block("crimson_planks"),
            new Block("warped_planks"),
            new Block("bedrock"),
            new Block("sand"),
            new Block("red_sand"),
            new Block("gravel"),
            new Block("coal_ore"),
            new Block("deepslate_coal_ore"),
            new Block("iron_ore"),
            new Block("deepslate_iron_ore"),
            new Block("copper_ore"),
            new Block("deepslate_copper_ore"),
            new Block("gold_ore"),
            new Block("deepslate_gold_ore"),
            new Block("redstone_ore"),
            new Block("deepslate_redstone_ore"),
            new Block("emerald_ore"),
            new Block("deepslate_emerald_ore"),
            new Block("lapis_ore"),
            new Block("deepslate_lapis_ore"),
            new Block("diamond_ore"),
            new Block("deepslate_diamond_ore"),
            new Block("nether_gold_ore"),
            new Block("nether_quartz_ore"),
            new Block("ancient_debris"),
            new Block("coal_block"),
            new Block("raw_iron_block"),
            new Block("raw_copper_block"),
            new Block("raw_gold_block"),
            new Block("amethyst_block"),
            new Block("budding_amethyst"),
            new Block("iron_block"),
            new Block("copper_block"),
            new Block("gold_block"),
            new Block("diamond_block"),
            new Block("netherite_block"),
            new Block("exposed_copper"),
            new Block("weathered_copper"),
            new Block("weathered_copper"),
            new Block("cut_copper"),
            new Block("exposed_cut_copper"),
            new Block("weathered_cut_copper"),
            new Block("oxidized_cut_copper"),
            new Block("oak_log"),
            new Block("spruce_log"),
            new Block("birch_log"),
            new Block("jungle_log"),
            new Block("acacia_log"),
            new Block("dark_oak_log"),
            new Block("mangrove_log"),
            new Block("mangrove_roots"),
            new Block("muddy_mangrove_roots"),
            new Block("crimson_stem"),
            new Block("warped_stem"),
            new Block("stripped_oak_log"),
            new Block("stripped_spruce_log"),
            new Block("stripped_birch_log"),
            new Block("stripped_jungle_log"),
            new Block("stripped_acacia_log"),
            new Block("stripped_dark_oak_log"),
            new Block("stripped_mangrove_log"),
            new Block("stripped_crimson_stem"),
            new Block("stripped_warped_stem"),
            new Block("sponge"),
            new Block("wet_sponge"),
            new Block("glass"),
            new Block("tinted_glass"),
            new Block("lapis_block"),
            new Block("sandstone"),
            new Block("chiseled_sandstone"),
            new Block("cut_sandstone"),
            new Block("white_wool"),
            new Block("orange_wool"),
            new Block("magenta_wool"),
            new Block("light_blue_wool"),
            new Block("yellow_wool"),
            new Block("lime_wool"),
            new Block("pink_wool"),
            new Block("gray_wool"),
            new Block("light_gray_wool"),
            new Block("cyan_wool"),
            new Block("purple_wool"),
            new Block("blue_wool"),
            new Block("brown_wool"),
            new Block("green_wool"),
            new Block("red_wool"),
            new Block("black_wool"),
            new Block("smooth_stone"),
            new Block("bricks"),
            new Block("bookshelf"),
            new Block("mossy_cobblestone"),
            new Block("obsidian"),
            new Block("purpur_pillar"),
            new Block("ice"),
            new Block("clay"),
            new Block("pumpkin"),
            new Block("jack_o_lantern"),
            new Block("netherrack"),
            new Block("soul_sand"),
            new Block("soul_soil"),
            new Block("basalt"),
            new Block("polished_basalt"),
            new Block("smooth_basalt"),
            new Block("glowstone"),
            new Block("stone_bricks"),
            new Block("mossy_stone_bricks"),
            new Block("cracked_stone_bricks"),
            new Block("chiseled_stone_bricks"),
            new Block("packed_mud"),
            new Block("mud_bricks"),
            new Block("deepslate_bricks"),
            new Block("cracked_deepslate_bricks"),
            new Block("deepslate_tiles"),
            new Block("cracked_deepslate_tiles"),
            new Block("chiseled_deepslate"),
            new Block("reinforced_deepslate"),
            new Block("melon"),
            new Block("mycelium"),
            new Block("nether_bricks"),
            new Block("cracked_nether_bricks"),
            new Block("chiseled_nether_bricks"),
            new Block("end_stone"),
            new Block("end_stone_bricks"),
            new Block("emerald_block"),
            new Block("chiseled_quartz_block"),
            new Block("quartz_block"),
            new Block("quartz_pillar"),
            new Block("white_terracotta"),
            new Block("orange_terracotta"),
            new Block("magenta_terracotta"),
            new Block("light_blue_terracotta"),
            new Block("yellow_terracotta"),
            new Block("lime_terracotta"),
            new Block("pink_terracotta"),
            new Block("gray_terracotta"),
            new Block("light_gray_terracotta"),
            new Block("cyan_terracotta"),
            new Block("purple_terracotta"),
            new Block("blue_terracotta"),
            new Block("brown_terracotta"),
            new Block("green_terracotta"),
            new Block("red_terracotta"),
            new Block("black_terracotta"),
            new Block("hay_block"),
            new Block("terracotta"),
            new Block("packed_ice"),
            new Block("white_stained_glass"),
            new Block("orange_stained_glass"),
            new Block("magenta_stained_glass"),
            new Block("light_blue_stained_glass"),
            new Block("yellow_stained_glass"),
            new Block("lime_stained_glass"),
            new Block("pink_stained_glass"),
            new Block("gray_stained_glass"),
            new Block("light_gray_stained_glass"),
            new Block("cyan_stained_glass"),
            new Block("purple_stained_glass"),
            new Block("blue_stained_glass"),
            new Block("brown_stained_glass"),
            new Block("green_stained_glass"),
            new Block("red_stained_glass"),
            new Block("black_stained_glass"),
            new Block("prismarine"),
            new Block("prismarine_bricks"),
            new Block("dark_prismarine"),
            new Block("sea_lantern"),
            new Block("red_sandstone"),
            new Block("chiseled_red_sandstone"),
            new Block("cut_red_sandstone"),
            new Block("nether_wart_block"),
            new Block("warped_wart_block"),
            new Block("red_nether_bricks"),
            new Block("nether_wart_block"),
            new Block("warped_wart_block"),
            new Block("red_nether_bricks"),
            new Block("bone_block"),
            new Block("white_concrete"),
            new Block("orange_concrete"),
            new Block("magenta_concrete"),
            new Block("light_blue_concrete"),
            new Block("yellow_concrete"),
            new Block("lime_concrete"),
            new Block("pink_concrete"),
            new Block("gray_concrete"),
            new Block("light_gray_concrete"),
            new Block("cyan_concrete"),
            new Block("purple_concrete"),
            new Block("blue_concrete"),
            new Block("brown_concrete"),
            new Block("green_concrete"),
            new Block("red_concrete"),
            new Block("black_concrete"),
            new Block("white_concrete_powder"),
            new Block("orange_concrete_powder"),
            new Block("magenta_concrete_powder"),
            new Block("light_blue_concrete_powder"),
            new Block("yellow_concrete_powder"),
            new Block("lime_concrete_powder"),
            new Block("pink_concrete_powder"),
            new Block("gray_concrete_powder"),
            new Block("light_gray_concrete_powder"),
            new Block("cyan_concrete_powder"),
            new Block("purple_concrete_powder"),
            new Block("blue_concrete_powder"),
            new Block("brown_concrete_powder"),
            new Block("green_concrete_powder"),
            new Block("red_concrete_powder"),
            new Block("black_concrete_powder"),
            new Block("dead_tube_coral_block"),
            new Block("dead_brain_coral_block"),
            new Block("dead_bubble_coral_block"),
            new Block("dead_fire_coral_block"),
            new Block("dead_horn_coral_block"),
            new Block("tube_coral_block"),
            new Block("brain_coral_block"),
            new Block("bubble_coral_block"),
            new Block("fire_coral_block"),
            new Block("horn_coral_block"),
            new Block("blue_ice"),
            new Block("crying_obsidian"),
            new Block("blackstone"),
            new Block("gilded_blackstone"),
            new Block("polished_blackstone"),
            new Block("chiseled_polished_blackstone"),
            new Block("polished_blackstone_bricks"),
            new Block("cracked_polished_blackstone_bricks"),
        });

    }

}