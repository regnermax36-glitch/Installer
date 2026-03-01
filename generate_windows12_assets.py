#!/usr/bin/env python3
"""
Windows 12 2027 Asset Generator
Creates futuristic UI assets with neural networks, holographic effects, and quantum patterns
"""

import numpy as np
from PIL import Image, ImageDraw, ImageFilter, ImageFont
import math
import random
from pathlib import Path

class Windows12AssetGenerator:
    def __init__(self):
        self.output_dir = Path("Rectify11Installer/Resources/InstallerImages/Windows12_2027")
        self.output_dir.mkdir(exist_ok=True)
        
        # Color palettes for different themes
        self.neural_colors = {
            'primary': (15, 23, 42),
            'secondary': (30, 41, 59),
            'accent': (59, 130, 246),
            'glow': (96, 165, 250),
            'text': (248, 250, 252)
        }
        
        self.holographic_colors = {
            'primary': (17, 24, 39),
            'secondary': (31, 41, 55),
            'accent': (168, 85, 247),
            'highlight': (236, 72, 153),
            'shimmer': (59, 130, 246),
            'glow': (16, 185, 129)
        }
        
        self.quantum_colors = {
            'primary': (7, 89, 133),
            'secondary': (14, 116, 144),
            'accent': (6, 182, 212),
            'energy': (52, 211, 153),
            'particle': (167, 243, 208)
        }

    def create_neural_network_pattern(self, width, height, node_count=50):
        """Creates a neural network visualization pattern"""
        img = Image.new('RGBA', (width, height), (0, 0, 0, 0))
        draw = ImageDraw.Draw(img)
        
        # Generate random nodes
        nodes = []
        for _ in range(node_count):
            x = random.randint(50, width - 50)
            y = random.randint(50, height - 50)
            nodes.append((x, y))
        
        # Draw connections between nearby nodes
        for i, (x1, y1) in enumerate(nodes):
            for j, (x2, y2) in enumerate(nodes[i+1:], i+1):
                distance = math.sqrt((x2-x1)**2 + (y2-y1)**2)
                if distance < 150:  # Only connect nearby nodes
                    # Connection strength based on distance
                    alpha = int(255 * (1 - distance/150) * 0.3)
                    color = (*self.neural_colors['accent'], alpha)
                    draw.line([(x1, y1), (x2, y2)], fill=color, width=1)
        
        # Draw nodes
        for x, y in nodes:
            # Outer glow
            for r in range(8, 2, -1):
                alpha = int(255 * (8-r)/8 * 0.5)
                color = (*self.neural_colors['glow'], alpha)
                draw.ellipse([x-r, y-r, x+r, y+r], fill=color)
            
            # Inner node
            draw.ellipse([x-2, y-2, x+2, y+2], fill=self.neural_colors['accent'])
        
        return img

    def create_holographic_shimmer(self, width, height):
        """Creates a holographic shimmer effect"""
        img = Image.new('RGBA', (width, height), (0, 0, 0, 0))
        draw = ImageDraw.Draw(img)
        
        # Create diagonal shimmer lines
        for i in range(0, width + height, 20):
            # Rainbow gradient effect
            hue = (i / (width + height)) * 360
            r = int(255 * (1 + math.sin(math.radians(hue))) / 2)
            g = int(255 * (1 + math.sin(math.radians(hue + 120))) / 2)
            b = int(255 * (1 + math.sin(math.radians(hue + 240))) / 2)
            
            alpha = int(255 * 0.1)  # Very subtle
            color = (r, g, b, alpha)
            
            # Draw diagonal line
            draw.line([(i, 0), (i - height, height)], fill=color, width=2)
        
        # Add some sparkle points
        for _ in range(30):
            x = random.randint(0, width)
            y = random.randint(0, height)
            size = random.randint(1, 3)
            alpha = random.randint(100, 200)
            color = (255, 255, 255, alpha)
            draw.ellipse([x-size, y-size, x+size, y+size], fill=color)
        
        return img

    def create_quantum_field(self, width, height):
        """Creates a quantum field energy pattern"""
        img = Image.new('RGBA', (width, height), (0, 0, 0, 0))
        draw = ImageDraw.Draw(img)
        
        # Create energy field waves
        for wave in range(5):
            for x in range(0, width, 2):
                # Multiple sine waves for complexity
                y1 = height//2 + int(30 * math.sin(x * 0.02 + wave * 1.2))
                y2 = height//2 + int(20 * math.sin(x * 0.03 + wave * 0.8))
                y3 = height//2 + int(15 * math.sin(x * 0.05 + wave * 1.5))
                
                # Combine waves
                y_avg = (y1 + y2 + y3) // 3
                
                # Energy intensity based on wave interference
                intensity = abs(y1 - y2) + abs(y2 - y3)
                alpha = min(255, int(intensity * 2))
                
                color = (*self.quantum_colors['energy'], alpha)
                draw.point((x, y_avg), fill=color)
                
                # Add energy particles
                if random.random() < 0.01:
                    particle_size = random.randint(1, 2)
                    particle_color = (*self.quantum_colors['particle'], alpha)
                    draw.ellipse([x-particle_size, y_avg-particle_size, 
                                x+particle_size, y_avg+particle_size], 
                               fill=particle_color)
        
        return img

    def create_glassmorphism_background(self, width, height, theme='neural'):
        """Creates a glassmorphism background effect"""
        if theme == 'neural':
            colors = self.neural_colors
        elif theme == 'holographic':
            colors = self.holographic_colors
        else:
            colors = self.quantum_colors
        
        img = Image.new('RGBA', (width, height), (*colors['primary'], 255))
        
        # Add gradient overlay
        gradient = Image.new('RGBA', (width, height), (0, 0, 0, 0))
        draw = ImageDraw.Draw(gradient)
        
        for y in range(height):
            alpha = int(255 * (y / height) * 0.3)
            color = (*colors['secondary'], alpha)
            draw.line([(0, y), (width, y)], fill=color)
        
        img = Image.alpha_composite(img, gradient)
        
        # Add noise for texture
        noise = Image.new('RGBA', (width, height), (0, 0, 0, 0))
        noise_pixels = []
        for _ in range(width * height):
            if random.random() < 0.1:
                alpha = random.randint(5, 15)
                noise_pixels.append((255, 255, 255, alpha))
            else:
                noise_pixels.append((0, 0, 0, 0))
        
        noise.putdata(noise_pixels)
        img = Image.alpha_composite(img, noise)
        
        return img

    def generate_main_background(self):
        """Generates the main installer background"""
        width, height = 800, 600
        
        # Create base glassmorphism background
        bg = self.create_glassmorphism_background(width, height, 'neural')
        
        # Add neural network pattern
        neural_pattern = self.create_neural_network_pattern(width, height)
        bg = Image.alpha_composite(bg, neural_pattern)
        
        # Add subtle holographic shimmer
        shimmer = self.create_holographic_shimmer(width, height)
        bg = Image.alpha_composite(bg, shimmer)
        
        # Apply blur for depth
        bg = bg.filter(ImageFilter.GaussianBlur(radius=0.5))
        
        bg.save(self.output_dir / "main_2027.png", "PNG")
        print("✅ Generated main_2027.png")

    def generate_welcome_image(self):
        """Generates the welcome page image"""
        width, height = 400, 300
        
        # Create quantum field background
        bg = self.create_glassmorphism_background(width, height, 'quantum')
        quantum_field = self.create_quantum_field(width, height)
        bg = Image.alpha_composite(bg, quantum_field)
        
        # Add central glow effect
        glow = Image.new('RGBA', (width, height), (0, 0, 0, 0))
        draw = ImageDraw.Draw(glow)
        
        center_x, center_y = width // 2, height // 2
        for r in range(100, 0, -5):
            alpha = int(255 * (100 - r) / 100 * 0.1)
            color = (*self.quantum_colors['accent'], alpha)
            draw.ellipse([center_x - r, center_y - r, center_x + r, center_y + r], fill=color)
        
        bg = Image.alpha_composite(bg, glow)
        bg.save(self.output_dir / "welcome_2027.png", "PNG")
        print("✅ Generated welcome_2027.png")

    def generate_progress_image(self):
        """Generates the progress page image"""
        width, height = 400, 200
        
        # Create neural background
        bg = self.create_glassmorphism_background(width, height, 'neural')
        
        # Add animated-looking progress elements
        progress = Image.new('RGBA', (width, height), (0, 0, 0, 0))
        draw = ImageDraw.Draw(progress)
        
        # Progress bar visualization
        bar_width = width - 100
        bar_height = 20
        bar_x = 50
        bar_y = height // 2 - bar_height // 2
        
        # Background bar
        draw.rounded_rectangle([bar_x, bar_y, bar_x + bar_width, bar_y + bar_height], 
                             radius=10, fill=(*self.neural_colors['secondary'], 100))
        
        # Progress fill (75% for visual appeal)
        fill_width = int(bar_width * 0.75)
        draw.rounded_rectangle([bar_x, bar_y, bar_x + fill_width, bar_y + bar_height], 
                             radius=10, fill=self.neural_colors['accent'])
        
        # Add glow effect to progress bar
        for i in range(5):
            alpha = int(255 * (5 - i) / 5 * 0.3)
            color = (*self.neural_colors['glow'], alpha)
            draw.rounded_rectangle([bar_x - i, bar_y - i, bar_x + fill_width + i, bar_y + bar_height + i], 
                                 radius=10 + i, outline=color, width=1)
        
        bg = Image.alpha_composite(bg, progress)
        bg.save(self.output_dir / "progress_2027.png", "PNG")
        print("✅ Generated progress_2027.png")

    def generate_complete_image(self):
        """Generates the completion page image"""
        width, height = 400, 300
        
        # Create holographic background
        bg = self.create_glassmorphism_background(width, height, 'holographic')
        shimmer = self.create_holographic_shimmer(width, height)
        bg = Image.alpha_composite(bg, shimmer)
        
        # Add success checkmark effect
        check = Image.new('RGBA', (width, height), (0, 0, 0, 0))
        draw = ImageDraw.Draw(check)
        
        center_x, center_y = width // 2, height // 2
        
        # Outer glow ring
        for r in range(80, 60, -2):
            alpha = int(255 * (80 - r) / 20 * 0.5)
            color = (*self.holographic_colors['glow'], alpha)
            draw.ellipse([center_x - r, center_y - r, center_x + r, center_y + r], outline=color, width=2)
        
        # Inner success circle
        draw.ellipse([center_x - 60, center_y - 60, center_x + 60, center_y + 60], 
                    fill=(*self.holographic_colors['accent'], 100))
        
        # Checkmark (simplified)
        check_points = [
            (center_x - 20, center_y),
            (center_x - 5, center_y + 15),
            (center_x + 20, center_y - 15)
        ]
        draw.line(check_points[:2], fill=(255, 255, 255, 255), width=4)
        draw.line(check_points[1:], fill=(255, 255, 255, 255), width=4)
        
        bg = Image.alpha_composite(bg, check)
        bg.save(self.output_dir / "complete_2027.png", "PNG")
        print("✅ Generated complete_2027.png")

    def generate_all_assets(self):
        """Generates all Windows 12 2027 UI assets"""
        print("🚀 Generating Windows 12 2027 UI Assets...")
        
        self.generate_main_background()
        self.generate_welcome_image()
        self.generate_progress_image()
        self.generate_complete_image()
        
        print("✨ All Windows 12 2027 assets generated successfully!")

if __name__ == "__main__":
    generator = Windows12AssetGenerator()
    generator.generate_all_assets()

