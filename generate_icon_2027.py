#!/usr/bin/env python3
"""
Windows 12 2027 Icon Generator
Creates a futuristic application icon with holographic effects
"""

from PIL import Image, ImageDraw, ImageFilter
import math

def create_windows12_icon():
    """Creates a futuristic Windows 12 2027 application icon"""
    
    # Icon sizes to generate
    sizes = [16, 32, 48, 64, 128, 256]
    
    for size in sizes:
        # Create base image
        img = Image.new('RGBA', (size, size), (0, 0, 0, 0))
        draw = ImageDraw.Draw(img)
        
        # Colors
        primary = (15, 23, 42)      # Deep neural blue
        accent = (59, 130, 246)     # Electric blue
        glow = (96, 165, 250)       # Light electric blue
        highlight = (168, 85, 247)  # Purple accent
        
        center = size // 2
        
        # Create main geometric shape (hexagon with depth)
        hex_radius = size // 3
        
        # Background glow
        for r in range(hex_radius + 10, hex_radius - 5, -1):
            alpha = int(255 * (hex_radius + 10 - r) / 15 * 0.3)
            color = (*glow, alpha)
            
            # Create hexagon points
            points = []
            for i in range(6):
                angle = i * math.pi / 3
                x = center + r * math.cos(angle)
                y = center + r * math.sin(angle)
                points.append((x, y))
            
            if len(points) >= 3:
                draw.polygon(points, fill=color)
        
        # Main hexagon
        hex_points = []
        for i in range(6):
            angle = i * math.pi / 3
            x = center + hex_radius * math.cos(angle)
            y = center + hex_radius * math.sin(angle)
            hex_points.append((x, y))
        
        # Fill main shape
        draw.polygon(hex_points, fill=accent)
        
        # Inner geometric pattern
        inner_radius = hex_radius // 2
        inner_points = []
        for i in range(6):
            angle = i * math.pi / 3 + math.pi / 6  # Offset by 30 degrees
            x = center + inner_radius * math.cos(angle)
            y = center + inner_radius * math.sin(angle)
            inner_points.append((x, y))
        
        draw.polygon(inner_points, fill=highlight)
        
        # Central core
        core_radius = max(2, size // 12)
        draw.ellipse([center - core_radius, center - core_radius, 
                     center + core_radius, center + core_radius], 
                    fill=(255, 255, 255))
        
        # Add connecting lines for tech feel
        if size >= 32:
            line_color = (*primary, 180)
            for i in range(6):
                angle = i * math.pi / 3
                start_x = center + (hex_radius - 5) * math.cos(angle)
                start_y = center + (hex_radius - 5) * math.sin(angle)
                end_x = center + inner_radius * math.cos(angle + math.pi / 6)
                end_y = center + inner_radius * math.sin(angle + math.pi / 6)
                draw.line([(start_x, start_y), (end_x, end_y)], fill=line_color, width=max(1, size // 64))
        
        # Apply subtle blur for modern look
        if size >= 64:
            img = img.filter(ImageFilter.GaussianBlur(radius=0.5))
        
        # Save individual size
        img.save(f"Rectify11Installer/r11_2027_{size}.png", "PNG")
        print(f"✅ Generated {size}x{size} icon")
    
    # Create ICO file with multiple sizes
    images = []
    for size in [16, 32, 48, 64, 128, 256]:
        img = Image.open(f"Rectify11Installer/r11_2027_{size}.png")
        images.append(img)
    
    # Save as ICO
    images[0].save("Rectify11Installer/r11_2027.ico", format='ICO', sizes=[(s, s) for s in sizes])
    print("✅ Generated r11_2027.ico with multiple sizes")
    
    print("🎨 Windows 12 2027 icon generation complete!")

if __name__ == "__main__":
    create_windows12_icon()

