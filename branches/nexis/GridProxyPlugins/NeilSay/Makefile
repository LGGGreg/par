
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=../..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Package/Plugins/NeilSay.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../bin/GridProxy.dll
BUILD_DIR = ../../Package/Plugins

OPENMETAVERSE_HTTP_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.Http.dll
OPENMETAVERSE_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.dll
OPENMETAVERSE_STRUCTUREDDATA_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.StructuredData.dll
GRIDPROXY_DLL_SOURCE=../../bin/GridProxy.dll
HTTPSERVER_DLL_SOURCE=../../OpenMetaverse/HttpServer.dll
OPENMETAVERSE_UTILITIES_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.Utilities.dll
GRIDPROXY_DLL_MDB_SOURCE=../../bin/GridProxy.dll.mdb
GRIDPROXY_DLL_MDB=$(BUILD_DIR)/GridProxy.dll.mdb
OPENMETAVERSETYPES_DLL_SOURCE=../../OpenMetaverse/OpenMetaverseTypes.dll
OPENMETAVERSE_DLL_CONFIG_SOURCE=../../OpenMetaverse/OpenMetaverse.dll.config
NEILSAY_DLL_MDB_SOURCE=../../Package/Plugins/NeilSay.dll.mdb
NEILSAY_DLL_MDB=$(BUILD_DIR)/NeilSay.dll.mdb

endif

ifeq ($(CONFIG),RELEASE)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/Neilsay.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../bin/GridProxy.dll
BUILD_DIR = bin/Release

OPENMETAVERSE_HTTP_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.Http.dll
OPENMETAVERSE_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.dll
OPENMETAVERSE_STRUCTUREDDATA_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.StructuredData.dll
GRIDPROXY_DLL_SOURCE=../../bin/GridProxy.dll
HTTPSERVER_DLL_SOURCE=../../OpenMetaverse/HttpServer.dll
OPENMETAVERSE_UTILITIES_DLL_SOURCE=../../OpenMetaverse/OpenMetaverse.Utilities.dll
GRIDPROXY_DLL_MDB=
OPENMETAVERSETYPES_DLL_SOURCE=../../OpenMetaverse/OpenMetaverseTypes.dll
OPENMETAVERSE_DLL_CONFIG_SOURCE=../../OpenMetaverse/OpenMetaverse.dll.config
NEILSAY_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(OPENMETAVERSE_HTTP_DLL) \
	$(OPENMETAVERSE_DLL) \
	$(OPENMETAVERSE_STRUCTUREDDATA_DLL) \
	$(GRIDPROXY_DLL) \
	$(HTTPSERVER_DLL) \
	$(OPENMETAVERSE_UTILITIES_DLL) \
	$(GRIDPROXY_DLL_MDB) \
	$(OPENMETAVERSETYPES_DLL) \
	$(OPENMETAVERSE_DLL_CONFIG) \
	$(NEILSAY_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(NEILSAY_PC)  


RESGEN=resgen2

OPENMETAVERSE_HTTP_DLL = $(BUILD_DIR)/OpenMetaverse.Http.dll
OPENMETAVERSE_DLL = $(BUILD_DIR)/OpenMetaverse.dll
NEILSAY_PC = $(BUILD_DIR)/neilsay.pc
OPENMETAVERSE_STRUCTUREDDATA_DLL = $(BUILD_DIR)/OpenMetaverse.StructuredData.dll
GRIDPROXY_DLL = $(BUILD_DIR)/GridProxy.dll
HTTPSERVER_DLL = $(BUILD_DIR)/HttpServer.dll
OPENMETAVERSE_UTILITIES_DLL = $(BUILD_DIR)/OpenMetaverse.Utilities.dll
OPENMETAVERSETYPES_DLL = $(BUILD_DIR)/OpenMetaverseTypes.dll
OPENMETAVERSE_DLL_CONFIG = $(BUILD_DIR)/OpenMetaverse.dll.config

FILES = \
	AssemblyInfo.cs \
	NeilSay.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	neilsay.pc.in 

REFERENCES =  \
	System

DLL_REFERENCES =  \
	../../OpenMetaverse/OpenMetaverse.dll \
	../../OpenMetaverse/OpenMetaverse.StructuredData.dll \
	../../OpenMetaverse/OpenMetaverseTypes.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,OPENMETAVERSE_HTTP_DLL))
$(eval $(call emit-deploy-target,OPENMETAVERSE_DLL))
$(eval $(call emit-deploy-wrapper,NEILSAY_PC,neilsay.pc))
$(eval $(call emit-deploy-target,OPENMETAVERSE_STRUCTUREDDATA_DLL))
$(eval $(call emit-deploy-target,GRIDPROXY_DLL))
$(eval $(call emit-deploy-target,HTTPSERVER_DLL))
$(eval $(call emit-deploy-target,OPENMETAVERSE_UTILITIES_DLL))
$(eval $(call emit-deploy-target,GRIDPROXY_DLL_MDB))
$(eval $(call emit-deploy-target,OPENMETAVERSETYPES_DLL))
$(eval $(call emit-deploy-target,OPENMETAVERSE_DLL_CONFIG))


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
	$(call cp,$(OPENMETAVERSE_HTTP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSE_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(libdir)/pkgconfig'
	$(call cp,$(NEILSAY_PC),$(DESTDIR)$(libdir)/pkgconfig)
	$(call cp,$(OPENMETAVERSE_STRUCTUREDDATA_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GRIDPROXY_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(HTTPSERVER_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSE_UTILITIES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GRIDPROXY_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSETYPES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(OPENMETAVERSE_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(NEILSAY_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSE_HTTP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSE_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(NEILSAY_PC),$(DESTDIR)$(libdir)/pkgconfig)
	$(call rm,$(OPENMETAVERSE_STRUCTUREDDATA_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GRIDPROXY_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(HTTPSERVER_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSE_UTILITIES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GRIDPROXY_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSETYPES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(OPENMETAVERSE_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(NEILSAY_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
