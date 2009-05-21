
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=../..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Package/Plugins/Shadow-GTK.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../bin/GridProxy.dll \
	../../Twofish/bin/Debug/Twofish.dll
BUILD_DIR = ../../Package/Plugins

SHADOW_GTK_DLL_MDB_SOURCE=../../Package/Plugins/Shadow-GTK.dll.mdb
SHADOW_GTK_DLL_MDB=$(BUILD_DIR)/Shadow-GTK.dll.mdb
OPENMETAVERSE_HTTP_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.Http.dll
OPENMETAVERSE_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.dll
OPENMETAVERSE_STRUCTUREDDATA_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.StructuredData.dll
GRIDPROXY_DLL_SOURCE=../../bin/GridProxy.dll
HTTPSERVER_DLL_SOURCE=../../OpenMetaverse/HttpServer.dll
TWOFISH_DLL_SOURCE=../../Twofish/bin/Debug/Twofish.dll
OPENMETAVERSE_UTILITIES_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.Utilities.dll
GRIDPROXY_DLL_MDB_SOURCE=../../bin/GridProxy.dll.mdb
GRIDPROXY_DLL_MDB=$(BUILD_DIR)/GridProxy.dll.mdb
OPENMETAVERSETYPES_DLL_SOURCE=../../OpenMetaverse/OpenMetaverseTypes.dll
OPENMETAVERSE_DLL_CONFIG_SOURCE=../../OpenMetaverse/OpenMetaverse.dll.config
TWOFISH_DLL_MDB_SOURCE=../../Twofish/bin/Debug/Twofish.dll.mdb
TWOFISH_DLL_MDB=$(BUILD_DIR)/Twofish.dll.mdb

endif

ifeq ($(CONFIG),RELEASE)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = ../PlugComb-GTK/Shadow-GTK/bin/Release/Shadow-GTK.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../bin/GridProxy.dll \
	../../Twofish/bin/Release/Twofish.dll
BUILD_DIR = ../PlugComb-GTK/Shadow-GTK/bin/Release

SHADOW_GTK_DLL_MDB=
OPENMETAVERSE_HTTP_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.Http.dll
OPENMETAVERSE_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.dll
OPENMETAVERSE_STRUCTUREDDATA_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.StructuredData.dll
GRIDPROXY_DLL_SOURCE=../../bin/GridProxy.dll
HTTPSERVER_DLL_SOURCE=../../OpenMetaverse/HttpServer.dll
TWOFISH_DLL_SOURCE=../../Twofish/bin/Release/Twofish.dll
OPENMETAVERSE_UTILITIES_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.Utilities.dll
GRIDPROXY_DLL_MDB=
OPENMETAVERSETYPES_DLL_SOURCE=../../OpenMetaverse/OpenMetaverseTypes.dll
OPENMETAVERSE_DLL_CONFIG_SOURCE=../../OpenMetaverse/OpenMetaverse.dll.config
TWOFISH_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SHADOW_GTK_DLL_MDB) \
	$(OPENMETAVERSE_HTTP_DLL) \
	$(OPENMETAVERSE_DLL) \
	$(OPENMETAVERSE_STRUCTUREDDATA_DLL) \
	$(GRIDPROXY_DLL) \
	$(HTTPSERVER_DLL) \
	$(TWOFISH_DLL) \
	$(OPENMETAVERSE_UTILITIES_DLL) \
	$(GRIDPROXY_DLL_MDB) \
	$(OPENMETAVERSETYPES_DLL) \
	$(OPENMETAVERSE_DLL_CONFIG) \
	$(TWOFISH_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SHADOW_GTK_PC)  


RESGEN=resgen2

SHADOW_GTK_PC = $(BUILD_DIR)/shadow-gtk.pc
OPENMETAVERSE_HTTP_DLL = $(BUILD_DIR)/OpenMetaverse.Http.dll
OPENMETAVERSE_DLL = $(BUILD_DIR)/OpenMetaverse.dll
OPENMETAVERSE_STRUCTUREDDATA_DLL = $(BUILD_DIR)/OpenMetaverse.StructuredData.dll
GRIDPROXY_DLL = $(BUILD_DIR)/GridProxy.dll
HTTPSERVER_DLL = $(BUILD_DIR)/HttpServer.dll
TWOFISH_DLL = $(BUILD_DIR)/Twofish.dll
OPENMETAVERSE_UTILITIES_DLL = $(BUILD_DIR)/OpenMetaverse.Utilities.dll
OPENMETAVERSETYPES_DLL = $(BUILD_DIR)/OpenMetaverseTypes.dll
OPENMETAVERSE_DLL_CONFIG = $(BUILD_DIR)/OpenMetaverse.dll.config

FILES = \
	ShadowPlugin.cs \
	gtk-gui/generated.cs \
	ShadowFormGTK.cs \
	gtk-gui/PubComb.ShadowFormGTK.cs 

DATA_FILES = 

RESOURCES = \
	gtk-gui/gui.stetic 

EXTRAS = \
	ShadowPrefsForm2.cs \
	ShadowForm1.resx \
	ShadowPrefsForm2.resx \
	ShadowPrefsForm2.Designer.cs \
	ShadowForm1.cs \
	ShadowForm1.Designer.cs \
	shadow-gtk.pc.in 

REFERENCES =  \
	System.Data \
	System \
	System.Drawing \
	Mono.Posix \
	-pkg:gtk-sharp-2.0

DLL_REFERENCES =  \
	../../OpenMetaverse/OpenMetaverse.dll \
	../../OpenMetaverse/OpenMetaverseTypes.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-wrapper,SHADOW_GTK_PC,shadow-gtk.pc))
$(eval $(call emit-deploy-target,OPENMETAVERSE_HTTP_DLL))
$(eval $(call emit-deploy-target,OPENMETAVERSE_DLL))
$(eval $(call emit-deploy-target,OPENMETAVERSE_STRUCTUREDDATA_DLL))
$(eval $(call emit-deploy-target,GRIDPROXY_DLL))
$(eval $(call emit-deploy-target,HTTPSERVER_DLL))
$(eval $(call emit-deploy-target,TWOFISH_DLL))
$(eval $(call emit-deploy-target,OPENMETAVERSE_UTILITIES_DLL))
$(eval $(call emit-deploy-target,GRIDPROXY_DLL_MDB))
$(eval $(call emit-deploy-target,OPENMETAVERSETYPES_DLL))
$(eval $(call emit-deploy-target,OPENMETAVERSE_DLL_CONFIG))
$(eval $(call emit-deploy-target,TWOFISH_DLL_MDB))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'


$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(shell dirname $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-install-local-hook prefix=$(prefix)
	make install-satellite-assemblies prefix=$(prefix)
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)'
	$(call cp,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(SHADOW_GTK_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(libdir)/pkgconfig'
	$(call cp,$(SHADOW_GTK_PC),$(DESTDIR)$(libdir)/pkgconfig)
	$(call cp,$(OPENMETAVERSE_HTTP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSE_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSE_STRUCTUREDDATA_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GRIDPROXY_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(HTTPSERVER_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(TWOFISH_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSE_UTILITIES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GRIDPROXY_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSETYPES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSE_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(TWOFISH_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(SHADOW_GTK_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(SHADOW_GTK_PC),$(DESTDIR)$(libdir)/pkgconfig)
	$(call rm,$(OPENMETAVERSE_HTTP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSE_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSE_STRUCTUREDDATA_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GRIDPROXY_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(HTTPSERVER_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(TWOFISH_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSE_UTILITIES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GRIDPROXY_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSETYPES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSE_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(TWOFISH_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
